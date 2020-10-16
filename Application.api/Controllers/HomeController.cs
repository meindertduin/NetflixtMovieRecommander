using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;
using NetflixMoviesRecommender.api.Domain;

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
            return Ok(AppHttpContext.AppBaseUrl);
        }
    }
}