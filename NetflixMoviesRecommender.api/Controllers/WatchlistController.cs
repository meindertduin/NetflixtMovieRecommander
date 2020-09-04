using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetflixMoviesRecommender.api.Controllers
{
    [ApiController]
    [Route("api/watchlist")]
    public class WatchlistController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> watchLists)
        {
            long size = watchLists.Sum(f => f.Length);
            
            foreach (var watchlist in watchLists)
            {
                if (watchlist.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)
                    {
                        await watchlist.CopyToAsync(stream);
                    };
                }   
            }

            return Ok(new {count = watchLists.Count, size});
        }
    }
}