﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Forms;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NinjaNye.SearchExtensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/watchgroup")]
    public class WatchGroupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _ctx;
        private readonly IFileHandlerService _fileHandlerService;

        public WatchGroupController(UserManager<ApplicationUser> userManager
            , AppDbContext ctx, IFileHandlerService fileHandlerService)
        {
            _userManager = userManager;
            _ctx = ctx;
            _fileHandlerService = fileHandlerService;
        }

        [HttpGet]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetUserWatchGroups()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var watchGroups =  _ctx.WatchGroups
                .Where(x => x.OwnerId == user.Id && x.Deleted == false)
                .Include(x => x.Owner)
                .Include(x => x.Members)
                .Select(WatchGroupViewModel.Projection)
                .ToList();


            var json = JsonConvert.SerializeObject(watchGroups, new JsonSerializerSettings 
            { 
                ContractResolver = new CamelCasePropertyNamesContractResolver() 
            });
            
            // todo: map the watchgroup model in a viewmodel
            
            return Ok(json);

        }

        [HttpPost("watchlist-upload")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> UploadWatchList([FromForm] WatchGroupWatchListForm watchGroupWatchListForm)
        {
            var watchList = watchGroupWatchListForm.WatchList;
            var watchGroupId = watchGroupWatchListForm.watchGroupId;
            
            var watchGroup = await _ctx.WatchGroups.FindAsync(watchGroupId);


            if (watchGroup == null || watchList == null)
            {
                return StatusCode(413);
            }

            var savePath = await _fileHandlerService.SaveFile(watchList, new[] {".csv"});
            
            if (savePath == null)
            {
                return StatusCode(500);
            }
            
            var fileReader = new CsvReader();
            var pairs = fileReader.ReadCsvAsKeyValues(savePath);
            
            // processes the pairs and ads them to the watchgroup.watchitems
            var titles = pairs.Item1;
            List<string> shortTitles = new List<string>();
            
            // deprecading titles in shorter titles and removing duplicates
            for (int i = 0; i < titles.Count; i++)
            {
                var shortTitle = titles[i].Split(':');
                if (string.IsNullOrEmpty(shortTitle[0]) == false)
                {
                    shortTitles.Add(shortTitle[0]);
                }
            }

            shortTitles = shortTitles.Distinct().ToList();
            
            // convert titles to watch items that use the watchgroup id as foreign key
            var watchItems = new List<WatchItem>();
            
            for (int i = 0; i < shortTitles.Count; i++)
            {
                var watchItem = new WatchItem
                {
                    Title = shortTitles[i],
                    WatchGroupId = watchGroup.Id,
                };
                watchItems.Add(watchItem);
            }

            await _ctx.WatchItems.AddRangeAsync(watchItems);
            await _ctx.SaveChangesAsync();
            System.IO.File.Delete(savePath);

            return Ok();
        }
        
        [HttpPost("create")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> CreateGroup([FromBody] WatchGroupForm watchGroupForm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _ctx.UserProfiles.FindAsync(user.Id);
            
            // check if title already exists

            if (userProfile != null)
            {
                var watchGroup = new WatchGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = watchGroupForm.Title,
                    Description = watchGroupForm.Description,
                    Owner = userProfile,
                    AddedNames = watchGroupForm.AddedUsers.ToArray(),
                };
            
                // todo need to implement a way yo invite the existing users to the watchlist and let them accept
                await _ctx.WatchGroups.AddAsync(watchGroup);
                await _ctx.SaveChangesAsync();
                
                return Ok(watchGroup.Id);
            }

            return StatusCode(500);
        }

        [HttpPut("edit")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> EditGroupTitle([FromBody] UpdateWatchGroupForm watchGroupForm)
        {
            var watchGroup = await _ctx.WatchGroups.FindAsync(watchGroupForm.Id);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (watchGroup.OwnerId != user.Id)
            {
                return StatusCode(401);
            }

            watchGroup.Title = watchGroupForm.Title;
            watchGroup.AddedNames = watchGroupForm.AddedNames;
            watchGroup.Description = watchGroupForm.Description;
            
            await _ctx.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost("watch-item")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> AddWatchItem(string id, string title)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(title))
            {
                return BadRequest();
            }
            
            var watchGroup = await _ctx.WatchGroups.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (watchGroup.OwnerId != user.Id)
            {
                return StatusCode(401);
            }

            _ctx.WatchItems.Add(new WatchItem
            {
                WatchGroupId = id,
                Title = title,
            });

            await _ctx.SaveChangesAsync();
            
            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            var watchGroup = await _ctx.WatchGroups.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            if (watchGroup.OwnerId != user.Id)
            {
                return StatusCode(401);
            }

            watchGroup.Deleted = true;
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetRecommendations([FromRoute] string id, [FromBody] WatchGroupRecommendationForm recommendationForm)
        {
            int skipAmount = 25 * recommendationForm.Index;
            
            
            var watchGroup = await _ctx.WatchGroups
                .Where(x => x.Id == id)
                .Include(x => x.WatchItems)
                .FirstOrDefaultAsync();
            
            var watchedItems = watchGroup.WatchItems.ToList();
            var watchTitles = new List<string>();
            foreach (var watchedItem in watchedItems)
            {
                watchTitles.Add(watchedItem.Title);   
            }
            
            IQueryable<NetflixRecommended> randomRecommendations;
            
            if (recommendationForm.Genres.Length > 0)
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => recommendationForm.Type == "both" || x.Type == recommendationForm.Type)
                    .Where(x => watchTitles.All(p => x.Title != p))
                    .Where(x => x.Deleted == false)
                    .Search(x => x.Genres).Containing(recommendationForm.Genres)
                    .OrderBy(x => recommendationForm.Seed)
                    .Skip(skipAmount)
                    .Take(25);
            }
            else
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => recommendationForm.Type == "both" || x.Type == recommendationForm.Type)
                    .Where(x => watchTitles.All(p => x.Title != p))
                    .Where(x => x.Deleted == false)
                    .OrderBy(x => recommendationForm.Seed)
                    .Skip(skipAmount)
                    .Take(25);
            }
            
            var recommendations = new List<NetflixRecommended>();
            recommendations.AddRange(randomRecommendations);
            
            return Ok(recommendations);
        }
        
    }
}