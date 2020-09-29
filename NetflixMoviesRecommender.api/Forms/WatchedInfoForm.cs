using System.Collections.Generic;

namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchedInfoForm
    {
        public List<string> WatchedItems { get; set; }
        public string[] Genres { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }
    }
}