using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using Newtonsoft.Json;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/recommandation")]
    public class RecommandationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _ctx;
        private readonly HttpClient _client;

        public RecommandationController(IConfiguration configuration, IHttpClientFactory clientFactory, AppDbContext ctx)
        {
            _configuration = configuration;
            _ctx = ctx;
            _client = clientFactory.CreateClient();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRecommandation(string title)
        {
            string key = _configuration.GetValue<string>("OMDB_KEY");

            var res = await _client.GetAsync($"http://www.omdbapi.com/" +
                                       $"?apikey={_configuration.GetValue<string>("OMDB_KEY")}" +
                                       $"&t={title}");

            var resJsonString = await res.Content.ReadAsStringAsync();
            var recommended = JsonConvert.DeserializeObject<Recommended>(resJsonString);

            _ctx.Recommendations.Add(recommended);
            await _ctx.SaveChangesAsync();
            
            return Ok();
        }
        
        [HttpGet]
        public string Get()
        {
            string key = _configuration.GetValue<string>("OMDB_KEY");

            return key;
        }
    }
}