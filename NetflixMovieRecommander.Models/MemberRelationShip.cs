using System.ComponentModel.DataAnnotations;

namespace NetflixMovieRecommander.Models
{
    public class MemberRelationShip
    {
        [Key]
        public string MemberId { get; set; }
        public UserProfile Member { get; set; }
        public int WatchGroupId { get; set; }
        public WatchGroup WatchGroup { get; set; }
        
    }
}