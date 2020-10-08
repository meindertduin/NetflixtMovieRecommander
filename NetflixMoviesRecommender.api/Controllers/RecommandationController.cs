using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _ctx;

        public RecommandationController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        

        [HttpPost("watchlist")]
        public IActionResult Recommendation([FromBody] WatchedInfoForm watchedInfo)
        {
            var randomRecommendations = _ctx.NetflixRecommendations
                .AsNoTracking()
                .Where(x =>
                    watchedInfo.Type == "both" || x.Type == watchedInfo.Type &&
                    watchedInfo.WatchedItems.All(p => x.Title != p && 
                    watchedInfo.AlreadyLoaded.All(p => x.Id != p &&
                    x.Deleted == false)))
                .Search(x => x.Genres).Containing(watchedInfo.Genres)
                .OrderBy(x => Guid.NewGuid())
                .Take(25)
                .ToList();
            
            return Ok(randomRecommendations);
        }
    }
}