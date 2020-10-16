using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchGroupWatchListForm
    {
        public string watchGroupId { get; set; }
        public IFormFile WatchList { get; set; }
    }
}