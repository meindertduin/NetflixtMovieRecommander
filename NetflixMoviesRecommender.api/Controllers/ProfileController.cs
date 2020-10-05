using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;
using NetflixMoviesRecommender.api.AppDomain.Interfaces;
using NetflixMoviesRecommender.api.Domain;
using NetflixMoviesRecommender.api.Domain.Extensions;
using NetflixMoviesRecommender.api.Services;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IFileHandlerService _fileHandlerService;
        private readonly AppDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageProcessingService _imageProcessingService;
        
        private const int AVATAR_PIXEL_SIZE = 200;
        private const int MAX_AVATAR_BYTE_SIZE = 100_000_0;

        public ProfileController(IFileHandlerService fileHandlerService,
            AppDbContext ctx,
            UserManager<ApplicationUser> userManager,
            IImageProcessingService imageProcessingService)
        {
            _fileHandlerService = fileHandlerService;
            _ctx = ctx;
            _userManager = userManager;
            _imageProcessingService = imageProcessingService;
        }

        [HttpGet]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userProfile = _ctx.UserProfiles
                .Where(x => x.Id == user.Id)
                .Include(x => x.ProfileFiles)
                .FirstOrDefault();

            if (userProfile == null)
            {
                return Forbid();
            }

            var result = new UserProfileViewModel
            {
                UserName = userProfile.UserName,
                Id = userProfile.Id,
                AvatarUrl = AppHttpContext.AppBaseUrl + "/api/profile/picture/" + userProfile.Id,
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public IActionResult Get(string id)
        {
            var profile = _ctx.UserProfiles
                .Where(x => x.Id == id)
                .Include(x => x.ProfileFiles)
                .FirstOrDefault();

            if (profile == null)
            {
                return NotFound();
            }

            var result = new UserProfileViewModel
            {
                UserName = profile.UserName,
                Id = profile.Id,
                AvatarUrl = AppHttpContext.AppBaseUrl + "/api/profile/picture/" + profile.Id,
            };

            return Ok(result);
        }

        [HttpPost("picture")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> UploadPicture([FromForm] IFormFile picture)
        {
            if (picture == null || picture.IsImage() == false || picture.Length > MAX_AVATAR_BYTE_SIZE)
            {
                return BadRequest();
            }
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            var userProfile = _ctx.UserProfiles
                .Where(x => x.Id == user.Id)
                .Include(x => x.ProfileFiles)
                .FirstOrDefault();
            
            var savePath = await _fileHandlerService.SaveFile(picture);
            var resizeSucceeded = _imageProcessingService.TryResizeImage(savePath, AVATAR_PIXEL_SIZE,
                                                                                        out string outputPath,
                                                                                        out Task resizingTask);
            await resizingTask;
            
            if(resizeSucceeded == false)
            {
                return BadRequest();
            }
            
            await ParseImageToDatabase(picture, outputPath, savePath, userProfile);

            return Ok();
        }

        private async Task ParseImageToDatabase(IFormFile picture, string outputPath, string savePath,
            UserProfile userProfile)
        {
            var avatar = new ProfileFile
            {
                FileName = picture.FileName,
                FileType = FileType.Avatar,
                ContentType = picture.ContentType,
            };
            
            ReadProcessedImage(picture, outputPath, avatar);
            DeleteSavedImages(outputPath, savePath);
            await SetUserAvatar(userProfile, avatar);
        }

        private void ReadProcessedImage(IFormFile picture, string outputPath, ProfileFile avatar)
        {
            using (var reader = new BinaryReader(System.IO.File.OpenRead(outputPath)))
            {
                avatar.Content = reader.ReadBytes((int) picture.Length);
            }
        }

        private void DeleteSavedImages(string outputPath, string savePath)
        {
            System.IO.File.Delete(savePath);
            System.IO.File.Delete(outputPath);
        }
        
        private async Task SetUserAvatar(UserProfile userProfile, ProfileFile avatar)
        {
            var currentUserAvatar = userProfile.ProfileFiles.FirstOrDefault(x => x.FileType == FileType.Avatar);

            if (currentUserAvatar != null)
            {
                userProfile.ProfileFiles.Remove(currentUserAvatar);
                await _ctx.SaveChangesAsync();
            }

            userProfile.ProfileFiles.Add(avatar);

            _ctx.SaveChanges();
        }

        [HttpGet("picture/{id}")]
        public IActionResult GetPicture(string id)
        {
            var avatar = _ctx.ProfileFiles
                .Where(x => x.UserProfileId == id).FirstOrDefault(x => x.FileType == FileType.Avatar);

            if (avatar == null)
            {
                // return static image
                return NotFound();
            }

            return File(avatar.Content, "image/jpeg");
        }

        [HttpGet("find")]
        public IActionResult FindUsers(string searchTerm)
        {
            var profiles = _ctx.UserProfiles
                .Where(x => x.UserName.Contains(searchTerm))
                .OrderBy(x => x.UserName.Length)
                .Take(5);

            List<UserProfileViewModel> result = new List<UserProfileViewModel>();

            foreach (var userProfile in profiles)
            {
                result.Add(new UserProfileViewModel
                {
                    UserName = userProfile.UserName,
                    Id = userProfile.Id,
                    AvatarUrl = AppHttpContext.AppBaseUrl + "/api/profile/picture/" + userProfile.Id,
                });
            }

            return Ok(result);
        }
        
        [HttpGet("inbox")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetInbox()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            var inboxMessages = _ctx.InboxMessages
                .Where(x => x.ReceiverId == user.Id)
                .Include(x => x.Sender)
                .ToList();
            
            var result = new List<InboxMessageViewModel>();

            foreach (var inboxMessage in inboxMessages)
            {
                var sender = inboxMessage.Sender;
                
                if (sender != null)
                {
                    var message = MapInboxMessage(inboxMessage, sender);
                    if (message != null)
                    {
                        result.Add(message);
                    }
                }
            }
            return Ok(result);
        }

        private InboxMessageViewModel MapInboxMessage(InboxMessage inboxMessage, UserProfile sender)
        {
            IMessageViewModelFactory factory;
            
            switch (inboxMessage.MessageType)
            {
                case MessageType.General:
                    factory = new GeneralMessageViewModelFactory();
                    break;
                case MessageType.WatchGroupInvite:
                    factory = new InviteMessageViewModelFactory();
                    break;
                default:
                    return null;
            }

            return factory.CreateModel(inboxMessage, sender);
        }
        
        private WatchGroupInviteViewModel MapInviteAppendix(InboxMessage inboxMessage)
        {
            var invite = (WatchGroupInviteMessage) inboxMessage;
            var appendix = new WatchGroupInviteViewModel
            {
                GroupId = invite.GroupId,
                GroupTitle = invite.GroupTitle,
            };
            return appendix;
        }
        
        [HttpDelete("inbox")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> DeleteMessage([FromQuery] int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var message = _ctx.InboxMessages.FirstOrDefault(x => x.Id == id);

            if (message == null)
            {
                return NotFound();
            }

            if (message.ReceiverId != user.Id)
            {
                return Forbid();
            }

            _ctx.InboxMessages.Remove(message);
            _ctx.SaveChanges();

            return Ok();
        }
    }
}