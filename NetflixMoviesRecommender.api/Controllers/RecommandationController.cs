using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/recommandation")]
    public class RecommandationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _ctx;
        private readonly IRecommendedDatabaseParser _recommendedDatabaseParser;
        private readonly HttpClient _client;

        public RecommandationController(IConfiguration configuration, IHttpClientFactory clientFactory, AppDbContext ctx, IRecommendedDatabaseParser recommendedDatabaseParser)
        {
            _configuration = configuration;
            _ctx = ctx;
            _recommendedDatabaseParser = recommendedDatabaseParser;
            _client = clientFactory.CreateClient();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRecommandation(string title)
        {
            string key = _configuration.GetValue<string>("OMDB_KEY");

            try
            {
                var res = await _client.GetAsync($"http://www.omdbapi.com/" +
                                                 $"?apikey={_configuration.GetValue<string>("OMDB_KEY")}" +
                                                 $"&t={title}");

                var resJsonString = await res.Content.ReadAsStringAsync();
                var recommended = JsonConvert.DeserializeObject<Recommended>(resJsonString);

                if (string.IsNullOrEmpty(recommended.Title))
                {
                    return StatusCode(400);
                }
            
                await _recommendedDatabaseParser.StoreRecommendedToDatabase(recommended);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            
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