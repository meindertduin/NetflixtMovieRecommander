using System;

namespace NetflixMovieRecommander.Models
{
    public class WatchItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime DateWatched { get; set; }
    }
}