using System;
using System.Collections.Generic;
using System.Linq;
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
            List<NetflixRecommended> recommendations = new List<NetflixRecommended>();
            
            IQueryable<NetflixRecommended> randomRecommendations;

            if (watchedInfo.Genres.Length > 0)
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => watchedInfo.Type == "both" || x.Type == watchedInfo.Type)
                    .Where(x => watchedInfo.WatchedItems.All(p => x.Title != p))
                    .Where(x => watchedInfo.AlreadyLoaded.All(p => x.Id != p))
                    .Where(x => x.Deleted == false)
                    .Search(x => x.Genres).Containing(watchedInfo.Genres)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(25);
            }
            else
            {
                randomRecommendations = _ctx.NetflixRecommendations
                    .Where(x => watchedInfo.Type == "both" || x.Type == watchedInfo.Type)
                    .Where(x => watchedInfo.WatchedItems.All(p => x.Title != p))
                    .Where(x => watchedInfo.AlreadyLoaded.All(p => x.Id != p))
                    .Where(x => x.Deleted == false)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(25);
            }
            
            recommendations.AddRange(randomRecommendations);

            return Ok(recommendations);
        }
        
    }
}