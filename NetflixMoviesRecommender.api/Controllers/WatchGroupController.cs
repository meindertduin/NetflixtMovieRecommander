using Microsoft.AspNetCore.Mvc;
using NetflixMoviesRecommender.api.Forms;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/watchgroup")]
    public class WatchGroupController : ControllerBase
    {
        [HttpGet("create")]
        public IActionResult Get()
        {
            return Ok();
        }
        
        [HttpPost("create")]
        public IActionResult CreateGroup([FromBody] WatchGroupForm watchGroupForm)
        {
            
            return Ok();
        }
    }
}