using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace NetflixMovieRecommander.Models
{
    public class WatchGroup
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        
        public UserProfile Owner { get; set; }
        
        public string OwnerId { get; set; }
        
        
        public IList<WatchGroupUserProfile> Members { get; set; }
        
        public ICollection<WatchItem> WatchItems { get; set; }
        public string[] AddedNames { get; set; }

        public bool Deleted { get; set; } = false;
    }
}