using System;
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
                .Where(x => x.OwnerId == user.Id)
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
            var user = _userManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _ctx.UserProfiles.FindAsync(user.Result.Id);

            
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
    }
}