using Microsoft.AspNetCore.Mvc;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "Hello world";
        }
    }
}