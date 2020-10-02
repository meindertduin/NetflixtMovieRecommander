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

            var watchGroups = profile.OwnedWatchGroups
                .Where(x => x.Deleted ==false)
                .ToList();

            var memberGroupIds = profile.MemberWatchGroups
                .Select(x => x.WatchGroupId)
                .ToArray();
            
            var memberWatchGroups = _ctx.WatchGroups
                .Where(x => x.Deleted == false)
                .Where(x => memberGroupIds.Contains(x.Id))
                .Include(x => x.Owner)
                .Include(x => x.Members)
                .ToList();
            
            watchGroups.AddRange(memberWatchGroups);
            
            var result = await MapWatchGroups(watchGroups);

            return Ok(result);

        }

        private async Task<List<WatchGroupViewModel>> MapWatchGroups(List<WatchGroup> watchGroups)
        {
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

            return result;
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
            
            randomRecommendations = _ctx.NetflixRecommendations
                .Where(x => recommendationForm.Type == "both" || x.Type == recommendationForm.Type)
                .Where(x => watchTitles.All(p => x.Title != p))
                .Where(x => recommendationForm.AlreadyLoaded.All(p => x.Id != p))
                .Where(x => x.Deleted == false)
                .Search(x => x.Genres).Containing(recommendationForm.Genres)
                .OrderBy(x => Guid.NewGuid()).Take(recommendationsReturnAmount);
            
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


            if (AlreadySend(subject.InboxMessages, user.Id))
            {
                return Ok();
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

        private bool AlreadySend(ICollection<InboxMessage> inboxMessages, string userId)
        {
            foreach (var message in inboxMessages)
            {
                if (message.MessageType == MessageType.WatchGroupInvite && message.SenderId == userId)
                {
                    return true;
                }
            }

            return false;
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
            
            var message = await _ctx.InboxMessages.FindAsync(responseForm.MessageId) as WatchGroupInviteMessage;
            
            if (message == null || responseForm.InviterId != message.SenderId || profile.Id != message.ReceiverId)
            {
                return BadRequest();
            }
            
            var response = new InboxMessage
            {
                MessageType = MessageType.General,
                Title = $"Message from {user.UserName}",
                Sender = profile,
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
                    UserProfileId = profile.Id,
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