using System;
using System.ComponentModel.DataAnnotations;

namespace NetflixMovieRecommander.Models
{
    public class WatchGroupUserProfile
    {
        public string UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public string WatchGroupId { get; set; }
        public WatchGroup WatchGroup { get; set; }
        
    }
}