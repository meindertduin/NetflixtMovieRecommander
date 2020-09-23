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
            
            var profileAvatar = userProfile.ProfileFiles.FirstOrDefault(x => x.FileType == FileType.Avatar);

            var result = new UserProfileViewModel
            {
                UserName = userProfile.UserName, 
                Id = userProfile.Id, 
                Avatar = profileAvatar,
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

            var profileAvatar = profile.ProfileFiles.FirstOrDefault(x => x.FileType == FileType.Avatar);

            var result = new UserProfileViewModel
            {
                UserName = profile.UserName, 
                Id = profile.Id, 
                Avatar = profileAvatar,
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

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userProfile = await _ctx.UserProfiles.FindAsync(user.Id);
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
            }
            
            userProfile.ProfileFiles.Add(avatar);
            
            _ctx.SaveChanges();
            
            return Ok();
        }
    }
}