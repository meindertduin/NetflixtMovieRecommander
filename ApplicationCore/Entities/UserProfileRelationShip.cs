namespace NetflixMovieRecommander.Models
{
    public class UserProfileRelationShip
    {
        public string UserOneId { get; set; }
        public UserProfile UserOne { get; set; }
        public string UserTwoId { get; set; }
        public UserProfile UserTwo { get; set; }
    }
}