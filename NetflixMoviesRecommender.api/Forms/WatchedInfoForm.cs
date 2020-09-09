using System.Collections.Generic;

namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchedInfoForm
    {
        public List<string> WatchedItems { get; set; }
        public List<string> Genres { get; set; }
        public string Type { get; set; }
    }
}