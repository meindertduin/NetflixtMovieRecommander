using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public HomeController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public IActionResult Test()
        {
            var messages = _ctx.InboxMessages.ToList();
            foreach (var message in messages)
            {
                if (message.MessageType == MessageType.WatchGroupInvite)
                {
                    var invite = (WatchGroupInviteMessage) message;
                }
            }
            
            return Ok();
        }
    }
}