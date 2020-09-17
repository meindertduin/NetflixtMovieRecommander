using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetflixMoviesRecommender.api.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var user = HttpContext.User;
            return Ok();
        }
        
        [HttpGet("test")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public string TestAuth() => "test";
        
        [HttpGet("mod")]
        [Authorize(Policy = ApiConstants.Policies.Mod)]
        public string ModAuth() => "mod";
    }
    
    
}