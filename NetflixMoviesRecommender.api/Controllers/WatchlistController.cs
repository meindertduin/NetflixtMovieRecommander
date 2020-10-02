using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AppDbContext _ctx;
        private readonly IFileHandlerService _fileHandlerService;

        private const int MAX_WATCHLIST_SIZE = 500_000;

        public WatchlistController(IWebHostEnvironment env, 
            AppDbContext ctx,
            IFileHandlerService fileHandlerService)
        {
            _env = env;
            _ctx = ctx;
            _fileHandlerService = fileHandlerService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> watchLists)
        {
            if (watchLists.Count == 0)
            {
                return BadRequest();
            }
            
            List<string> savePaths = new List<string>();
            
            for (int i = 0; i < watchLists.Count; i++)
            {
                var watchList = watchLists[i];

                if (watchList.Length < MAX_WATCHLIST_SIZE)
                {
                    var savePath = await _fileHandlerService
                        .SaveFile(watchList, new[] {".scv"});

                    if (savePath == null)
                    {
                        return BadRequest();
                    }
                
                    savePaths.Add(savePath);
                }
            }
            
            List<string> refinedTitles = new List<string>();
            
            foreach (var savePath in savePaths)
            {
                var fileReader = new CsvReader();
                var pairs = fileReader.ReadKeyValues(savePath);
                var titles = pairs.Item1;
                
                for (int i = 0; i < titles.Count; i++)
                {
                    var shortTitle = titles[i].Split(':');
                    if (string.IsNullOrEmpty(shortTitle[0]) == false)
                    {
                        refinedTitles.Add(shortTitle[0]);
                    }

                    if (refinedTitles.Count > 1000)
                    {
                        break;
                    }
                }
                
                System.IO.File.Delete(savePath);
            }
            
            return Ok(refinedTitles.Distinct().ToList());
        }
    }
}