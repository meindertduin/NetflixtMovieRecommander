using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Forms;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;
using NinjaNye.SearchExtensions;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("/api/recommendation")]
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

        [HttpPost("watchlist")]
        public IActionResult Recommendation([FromBody] WatchedInfoForm watchedInfo)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            List<string> watchedItems = watchedInfo.WatchedItems;
            List<string> genres = watchedInfo.Genres;
            List<NetflixRecommended> recommendations = new List<NetflixRecommended>();
            
            Random rand = new Random();

            var skip = (int) (rand.NextDouble()) * _ctx.NetflixRecommendations.Count();
            IQueryable<NetflixRecommended> randomRecommendations;

            if (watchedInfo.Genres.Count > 0)
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => watchedInfo.Type == "both" || x.Type == watchedInfo.Type)
                    .Where(x => watchedItems.All(p => x.Title != p))
                    .Search(x => x.Genres).Containing(genres.ToArray())
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(5);
            }
            else
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => watchedInfo.Type == "both" || x.Type == watchedInfo.Type)
                    .Where(x => watchedItems.All(p => x.Title != p))
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(5);
            }
            
            recommendations.AddRange(randomRecommendations);

            return Ok(recommendations);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string title)
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
    }
}