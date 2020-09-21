using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NetflixMovieRecommander.Models
{
    public class UserProfile
    {
        [Key]
        public string Id { get; set; }

        [JsonIgnore]
        [IgnoreDataMember] 
        public ApplicationUser ApplicationUser { get; set; }
        
        [JsonIgnore]
        [IgnoreDataMember] 
        public ICollection<WatchGroup> OwnedWatchGroups { get; set; }
        
        [JsonIgnore]
        [IgnoreDataMember] 
        public IList<WatchGroupUserProfile> MemberWatchGroups { get; set; }
        
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
    }
}