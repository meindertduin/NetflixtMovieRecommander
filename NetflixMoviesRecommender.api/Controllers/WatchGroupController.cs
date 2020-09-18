using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Forms;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/watchgroup")]
    public class WatchGroupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _ctx;

        public WatchGroupController(UserManager<ApplicationUser> userManager, AppDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }
        
        [HttpGet("create")]
        public IActionResult Get()
        {
            return Ok();
        }
        
        [HttpPost("create")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> CreateGroup([FromBody] WatchGroupForm watchGroupForm)
        {
            // get the user and userprofile
            var user = _userManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _ctx.UserProfiles.FindAsync(user.Result.Id);

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
                
                return Ok();
            }

            return StatusCode(500);
        }
    }
}