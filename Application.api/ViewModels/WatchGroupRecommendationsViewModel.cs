using System;
using System.Collections.Generic;

namespace NetflixMovieRecommander.Models
{
    public class WatchGroupRecommendationsViewModel
    {
        public List<NetflixRecommended> NetflixRecommended { get; set; }
        public Guid Seed { get; set; }
        public int Index { get; set; }
    }
}