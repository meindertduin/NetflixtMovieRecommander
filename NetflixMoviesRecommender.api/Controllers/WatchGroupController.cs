using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;
using NetflixMoviesRecommender.api.AppDomain.QueryResults;
using NetflixMoviesRecommender.api.Domain;
using NetflixMoviesRecommender.api.Forms;
using NetflixMoviesRecommender.api.Services;
using NinjaNye.SearchExtensions;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/watchgroup")]
    public class WatchGroupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _ctx;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly ICsvReader _csvReader;

        private const int MAX_WATCH_ITEMS = 3000;
        private const int MAX_WATCHLIST_FILE_SIZE = 100_000_0;
        
        public WatchGroupController(UserManager<ApplicationUser> userManager
            , AppDbContext ctx, IFileHandlerService fileHandlerService,
            ICsvReader csvReader)
        {
            _userManager = userManager;
            _ctx = ctx;
            _fileHandlerService = fileHandlerService;
            _csvReader = csvReader;
        }

        [HttpGet]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetUserWatchGroups()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userWatchGroupsIdList = _ctx.UserProfiles
                .AsNoTracking()
                .Where(u => u.Id == user.Id)
                .Select(u => new WatchGroupIdsResult
                {
                    OnwGroupIds = u.OwnedWatchGroups.Select(o => o.Id).ToList(),
                    MemberGroupsIds = u.MemberWatchGroups.Select(m => m.WatchGroupId).ToList(),
                })
                .FirstOrDefault();


            var watchGroupResults = _ctx.WatchGroups
                .AsNoTracking()
                .Where(w => w.Deleted == false &&
                    userWatchGroupsIdList.OnwGroupIds.Contains(w.Id) || userWatchGroupsIdList.MemberGroupsIds.Contains(w.Id))
                .Select(w => new WatchGroupViewModel
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                    Owner = new UserProfileViewModel
                    {
                        Id = w.Owner.Id,
                        UserName = w.Owner.UserName,
                        AvatarUrl = "https://localhost:5001/api/profile/picture/" + w.Owner.Id,
                    },
                    Members = w.Members.Select(x => new UserProfileViewModel
                    {
                       Id = x.UserProfile.Id,
                       UserName = x.UserProfile.UserName,
                       AvatarUrl = AppHttpContext.AppBaseUrl + "/api/profile/picture/" + x.UserProfile.Id,
                    }).ToList(),
                    AddedNames = w.AddedNames,
                })
                .ToList();

            return Ok(watchGroupResults);
        }
        

        [HttpPost("watchlist-upload")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> UploadWatchList([FromForm] WatchGroupWatchListForm watchGroupWatchListForm)
        {
            var watchList = watchGroupWatchListForm.WatchList;
            var watchGroup = await _ctx.WatchGroups.FindAsync(watchGroupWatchListForm.watchGroupId);
            
            if (watchGroup == null || watchList == null || watchGroupWatchListForm.WatchList.Length > MAX_WATCHLIST_FILE_SIZE)
            {
                return BadRequest();
            }

            var savePath = await _fileHandlerService.SaveFile(watchList, new[] {".csv"});
            
            if (savePath == null)
            {
                return BadRequest();
            }
            
            var pairs = _csvReader.ReadKeyValues(savePath);
            var titles = pairs.Item1;
            
            var shortenedTitles = ShortenTitles(titles);
            var distinctTitles = shortenedTitles.Distinct().ToList();
            
            var watchGroupItems = _ctx.WatchItems
                .Where(x => x.WatchGroupId == watchGroupWatchListForm.watchGroupId)
                .ToArray();
            
            var watchItems = MapWatchItemsOfTitles(distinctTitles, watchGroup, watchGroupItems);

            await SaveWatchItems(watchItems, watchGroupItems, savePath);

            return Ok();
        }
        
        private List<string> ShortenTitles(List<string> titles)
        {
            List<string> shortenedTitles = new List<string>();

            for (int i = 0; i < titles.Count; i++)
            {
                var shortTitle = titles[i].Split(':');
                if (string.IsNullOrEmpty(shortTitle[0]) == false)
                {
                    shortenedTitles.Add(shortTitle[0]);
                }
            }

            return shortenedTitles;
        }
        
        private List<WatchItem> MapWatchItemsOfTitles(List<string> distinctTitles, WatchGroup watchGroup, WatchItem[] watchGroupItems)
        {
            var watchItems = new List<WatchItem>();

            for (int i = 0; i < distinctTitles.Count; i++)
            {
                var watchItem = new WatchItem
                {
                    Title = distinctTitles[i],
                    WatchGroupId = watchGroup.Id,
                };
                if (watchGroupItems.Contains(watchItem) == false)
                {
                    watchItems.Add(watchItem);
                }
            }

            return watchItems;
        }
        
        
        private async Task SaveWatchItems(List<WatchItem> watchItems, WatchItem[] watchGroupItems, string savePath)
        {
            if (watchItems.Count + watchGroupItems.Length < MAX_WATCH_ITEMS)
            {
                await _ctx.WatchItems.AddRangeAsync(watchItems);
            }

            await _ctx.SaveChangesAsync();
            System.IO.File.Delete(savePath);
        }

        [HttpPost("create")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> CreateGroup([FromBody] WatchGroupForm watchGroupForm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _ctx.UserProfiles.FindAsync(user.Id);
            
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
        public async Task<IActionResult> AddWatchItem([FromBody] WatchItemAdditionForm watchItemForm)
        {
            if (string.IsNullOrEmpty(watchItemForm.GroupId) || string.IsNullOrEmpty(watchItemForm.WatchItemTitle))
            {
                return BadRequest();
            }
            
            var watchGroup = await _ctx.WatchGroups.FindAsync(watchItemForm.GroupId);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (watchGroup.OwnerId != user.Id)
            {
                return StatusCode(401);
            }

            _ctx.WatchItems.Add(new WatchItem
            {
                WatchGroupId = watchItemForm.GroupId,
                Title = watchItemForm.WatchItemTitle,
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

            if (watchGroup == null)
            {
                return BadRequest();
            }
            
            if (watchGroup.OwnerId != user.Id)
            {
                return Forbid();
            }

            _ctx.WatchGroups.Remove(watchGroup);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetRecommendations([FromRoute] string id, [FromBody] WatchGroupRecommendationForm recommendationForm)
        {
            int recommendationsReturnAmount = 25;

            var watchTitles = _ctx.WatchGroups
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(w => w.Title)
                .ToArray();
            
            var randomRecommendations = _ctx.NetflixRecommendations
                .AsNoTracking()
                .Where(x => 
                    recommendationForm.Type == "both" || x.Type == recommendationForm.Type &&
                    watchTitles.All(p => x.Title != p) &&
                    recommendationForm.AlreadyLoaded.All(p => x.Id != p) && 
                    x.Deleted == false)
                .Search(x => x.Genres).Containing(recommendationForm.Genres)
                .OrderBy(x => Guid.NewGuid()).Take(recommendationsReturnAmount)
                .ToList();
            
            return Ok(randomRecommendations);
        }

        [HttpPost("invite")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> Invite([FromBody] WatchGroupInviteForm groupInvite)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var alreadySend = _ctx.UserProfiles
                .Select(x => new UserProfile
                {
                    SendMessages = x.SendMessages,
                })
                .FirstOrDefault(u => u.SendMessages.Any(i =>
                    i.ReceiverId == user.Id && i.MessageType == MessageType.WatchGroupInvite));

            if (alreadySend == null)
            {
                return Ok();
            }
            
            var watchGroup = await _ctx.WatchGroups.FindAsync(groupInvite.GroupId);
            
            if (watchGroup == null)
            {
                return NotFound();
            }
            
            var invite = new WatchGroupInviteMessage
            {
                MessageType = MessageType.WatchGroupInvite,
                Title = $"Invite from {user.UserName}",
                Description = $"{user.UserName} invited you to join watch group: {watchGroup.Title}",
                SenderId = user.Id,
                DateSend = DateTime.Now,
                ReceiverId = groupInvite.SubjectId,
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

            var message = await _ctx.InboxMessages.FindAsync(responseForm.MessageId) as WatchGroupInviteMessage;
            
            if (message == null || responseForm.InviterId != message.SenderId || user.Id != message.ReceiverId)
            {
                return BadRequest();
            }
            
            var response = new InboxMessage
            {
                MessageType = MessageType.General,
                Title = $"Message from {user.UserName}",
                SenderId = user.Id,
                DateSend = DateTime.Now,
                ReceiverId = message.SenderId,
            };
            
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
                    UserProfileId = user.Id,
                });

                response.Description = $"{user.UserName} has accepted your invite";
            }
            else
            {
                response.Description = $"{user.UserName} has declined your invite";
            }
            
            _ctx.InboxMessages.Add(response);
            _ctx.InboxMessages.Remove(message);
            _ctx.SaveChanges();
            
            return Ok();
        }
    }
}