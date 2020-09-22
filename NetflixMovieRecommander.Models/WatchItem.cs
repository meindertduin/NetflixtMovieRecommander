using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetflixMovieRecommander.Models
{
    public class WatchItem
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        public string Title { get; set; }

        public WatchGroup WatchGroup { get; set; }
        public string WatchGroupId { get; set; }
        
    }
}