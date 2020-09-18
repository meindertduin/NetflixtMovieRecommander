using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetflixMovieRecommander.Models
{
    public class UserProfile
    {
        [Key]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<WatchGroup> OwnedWatchGroups { get; set; }
        
        public IList<WatchGroupUserProfile> MemberWatchGroups { get; set; }
        
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
    }
}