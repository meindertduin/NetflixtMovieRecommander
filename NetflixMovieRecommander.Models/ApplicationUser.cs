using Microsoft.AspNetCore.Identity;

namespace NetflixMovieRecommander.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName) : base(userName)
        {
                
        }
        public UserProfile UserProfile { get; set; }
    }
}