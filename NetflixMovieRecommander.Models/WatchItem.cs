using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetflixMovieRecommander.Models
{
    public class WatchItem
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }

        public WatchGroup WatchGroup { get; set; }
        public string WatchGroupId { get; set; }
        
    }
}