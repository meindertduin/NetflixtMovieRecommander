namespace NetflixMovieRecommander.Models
{
    public class UserInboxWatchGroupInviteMessage : UserInboxMessageBase
    {
        public string GroupId { get; set; }
        public string GroupTitle { get; set; }
    }
}