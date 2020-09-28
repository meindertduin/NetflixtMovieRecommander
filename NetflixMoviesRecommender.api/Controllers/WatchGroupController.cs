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
using Microsoft.EntityFrameworkCore.Internal;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;
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
            var profile = _ctx.UserProfiles
                .Where(x => x.Id == user.Id)
                .Include(x => x.OwnedWatchGroups)
                .ThenInclude(x => x.Members)
                .Include(x => x.MemberWatchGroups)
                .FirstOrDefault();


            if (profile == null)
            {
                return Forbid();
            }

            var watchGroups = profile.OwnedWatchGroups.ToList();

            var memberGroupIds = profile.MemberWatchGroups.Select(x => x.WatchGroupId).ToArray();
            
            var memberWatchGroups = _ctx.WatchGroups
                .Where(x => memberGroupIds.Contains(x.Id))
                .Include(x => x.Owner)
                .Include(x => x.Members)
                .ToList();
            
            watchGroups.AddRange(memberWatchGroups);
            
            var result = new List<WatchGroupViewModel>();

            foreach (var watchGroup in watchGroups)
            {
                var members = new List<UserProfileViewModel>();
                var owner = new UserProfileViewModel
                {
                    Id = watchGroup.Owner.Id,
                    UserName = watchGroup.Owner.UserName,
                    AvatarUrl = "https://localhost:5001/api/profile/picture/" + watchGroup.Owner.Id,
                };
                foreach (var member in watchGroup.Members)
                {
                    var memberProfile = await _userManager.FindByIdAsync(member.UserProfileId);
                    if (memberProfile.UserName != null)
                    {
                        members.Add(new UserProfileViewModel
                        {
                            Id = member.UserProfileId,
                            UserName = memberProfile.UserName,
                            AvatarUrl = "https://localhost:5001/api/profile/picture/" + member.UserProfileId,
                        });
                    }
                }
                
                result.Add(new WatchGroupViewModel
                {
                    Id = watchGroup.Id,
                    Title = watchGroup.Title,
                    Description = watchGroup.Description,
                    Owner = owner,
                    Members = members,
                    AddedNames = watchGroup.AddedNames,
                });
            }
            
            return Ok(result);

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

        [HttpPost("invite")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> Invite([FromBody] WatchGroupInviteForm groupInvite)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profile = await _ctx.UserProfiles.FindAsync(user.Id);
            var subject = _ctx.UserProfiles
                .Include(x => x.InboxMessages)
                .FirstOrDefault(x => x.Id == groupInvite.SubjectId);
            
            var watchGroup = await _ctx.WatchGroups.FindAsync(groupInvite.GroupId);
            
            if (profile == null)
            {
                return Forbid();
            }

            if (watchGroup == null || subject == null)
            {
                return NotFound();
            }

            
            // validating if request has already been send
            foreach (var message in subject.InboxMessages)
            {
                if (message.MessageType == MessageType.WatchGroupInvite && message.SenderId == user.Id)
                {
                    return Ok();
                }
            }
            
            var invite = new WatchGroupInviteMessage
            {
                MessageType = MessageType.WatchGroupInvite,
                Title = $"Invite from {user.UserName}",
                Description = $"{user.UserName} invited you to join watch group: {watchGroup.Title}",
                Sender = profile,
                DateSend = DateTime.Now,
                Receiver = subject,
                GroupId = watchGroup.Id,
                GroupTitle = watchGroup.Title,
            };

            await _ctx.InboxMessages.AddAsync(invite);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("invite/response")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> Accept([FromBody] WatchGroupInviteResponseForm responseForm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profile = await _ctx.UserProfiles.FindAsync(user.Id);
            if (profile == null)
            {
                return Forbid();
            }
            
            // validating invite
            var message = await _ctx.InboxMessages.FindAsync(responseForm.MessageId) as WatchGroupInviteMessage;
            if (message == null || responseForm.InviterId != message.SenderId || profile.Id != message.ReceiverId)
            {
                return BadRequest();
            }
            
            if (responseForm.Accepted)
            {
                var watchGroup = _ctx.WatchGroups
                    .Where(x => x.Id == message.GroupId)
                    .Include(x => x.Members)
                    .FirstOrDefault();

                if (watchGroup == null)
                {
                    return BadRequest();
                }
                
                watchGroup.Members.Add(new WatchGroupUserProfile
                {
                    WatchGroupId = watchGroup.Id,
                    UserProfileId = profile.Id,
                });
                
                var accept = new InboxMessage
                {
                    MessageType = MessageType.General,
                    Title = $"Message from {user.UserName}",
                    Description = $"{user.UserName} has accepted your invite",
                    Sender = profile,
                    DateSend = DateTime.Now,
                    ReceiverId = message.SenderId,
                };

                _ctx.InboxMessages.Add(accept);
                _ctx.InboxMessages.Remove(message);
                
                _ctx.SaveChanges();

                return Ok();
            }
            
            var decline = new InboxMessage
            {
                MessageType = MessageType.General,
                Title = $"Message from {user.UserName}",
                Description = $"{user.UserName} has declined your invite",
                Sender = profile,
                DateSend = DateTime.Now,
                ReceiverId = message.SenderId,
            };
            
            _ctx.InboxMessages.Add(decline);
            _ctx.InboxMessages.Remove(message);
            _ctx.SaveChanges();
            
            return Ok();
        }
    }
}