namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchGroupInviteResponseForm
    {
        public int MessageId { get; set; }
        public bool Accepted { get; set; }
        public string InviterId { get; set; }
    }
}