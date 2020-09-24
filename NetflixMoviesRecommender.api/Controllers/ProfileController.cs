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

        public ProfileController(IFileHandlerService fileHandlerService, 
            AppDbContext ctx, 
            UserManager<ApplicationUser> userManager)
        {
            _fileHandlerService = fileHandlerService;
            _ctx = ctx;
            _userManager = userManager;
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
                AvatarUrl = "https://localhost:5001/api/profile/picture/" + userProfile.Id,
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
                AvatarUrl = "https://localhost:5001/api/profile/picture/" + profile.Id,
            };
            
            return Ok(result);
        }
        
        [HttpPost("picture")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> UploadPicture([FromForm] IFormFile picture)
        {
            if (picture == null)
            {
                return BadRequest();
            }

            if (picture.IsImage() == false)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userProfile = _ctx.UserProfiles
                .Where(x => x.Id == user.Id)
                .Include(x => x.ProfileFiles)
                .FirstOrDefault();
            
            if (userProfile == null)
            {
                return Problem();
            }
            
            var avatar = new ProfileFile
            {
                FileName = picture.FileName,
                FileType = FileType.Avatar,
                ContentType = picture.ContentType,
            };


            using (var reader = new BinaryReader(picture.OpenReadStream()))
            {
                avatar.Content = reader.ReadBytes((int)picture.Length);
            };

            var currentUserAvatar = userProfile.ProfileFiles.FirstOrDefault(x => x.FileType == FileType.Avatar);
            
            if (currentUserAvatar != null)
            {
                userProfile.ProfileFiles.Remove(currentUserAvatar);
                await _ctx.SaveChangesAsync();
            }
            
            userProfile.ProfileFiles.Add(avatar);
            
            _ctx.SaveChanges();
            
            return Ok();
        }

        [HttpGet("picture/{id}")]
        public IActionResult GetPicture(string id)
        {
            var avatar = _ctx.ProfileFiles
                .Where(x => x.UserProfileId == id).
                FirstOrDefault(x => x.FileType == FileType.Avatar);

            if (avatar == null)
            {
                // return static image
                return NotFound();
            }
            
            return File(avatar.Content, "image/jpeg");
        }

        [HttpGet("find")]
        public IActionResult FindUser(string searchTerm)
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
                    AvatarUrl = "https://localhost:5001/api/profile/picture/" + userProfile.Id,
                });
            }
            
            return Ok(result);
        }
        
    }
}