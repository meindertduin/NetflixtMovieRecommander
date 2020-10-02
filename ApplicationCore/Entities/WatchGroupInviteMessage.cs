namespace NetflixMovieRecommander.Models
{
    public class WatchGroupInviteMessage : InboxMessage
    {
        public string GroupId { get; set; }
        public string GroupTitle { get; set; }
    }
}