using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Services;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("api/watchlist")]
    public class WatchlistController : ControllerBase
    {
        private IWebHostEnvironment _env;
        private readonly IWatchListFileParserService _watchListFileParserService;
        private readonly AppDbContext _ctx;

        public WatchlistController(IWebHostEnvironment env, 
            IWatchListFileParserService watchListFileParserService,
            AppDbContext ctx)
        {
            _env = env;
            _watchListFileParserService = watchListFileParserService;
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<WatchItem> Get()
        {
            return _ctx.WatchItems.ToList();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile watchList)
        {
            //long size = watchLists.Sum(f => f.Length);

            var mime = watchList.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);
            
            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await watchList.CopyToAsync(fileStream);
            }
            
            var fileReader = new CsvReader();
            var pairs = fileReader.ReadCsvAsKeyValues(savePath);

            var titles = pairs.Item1;
            List<string> refinedTitles = new List<string>();
            
            for (int i = 0; i < titles.Count; i++)
            {
                var shortTitle = titles[i].Split(':');
                if (string.IsNullOrEmpty(shortTitle[0]) == false)
                {
                    refinedTitles.Add(shortTitle[0]);
                }
            }

            return Ok(refinedTitles.Distinct().ToList());



        }
    }
}