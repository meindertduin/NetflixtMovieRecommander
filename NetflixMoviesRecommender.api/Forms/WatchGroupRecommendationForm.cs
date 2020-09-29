using System;

namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchGroupRecommendationForm
    {
        public string[] Genres { get; set; }
        public int Index { get; set; }
        public int[] AlreadyLoaded { get; set; }
        public string Type { get; set; }
    }
}