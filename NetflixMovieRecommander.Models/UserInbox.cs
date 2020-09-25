using System.Collections.Generic;

namespace NetflixMovieRecommander.Models
{
    public class UserInbox
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }

        public ICollection<UserInboxWatchGroupInviteMessage> WatchGroupInviteMessages { get; set; }
        public ICollection<UserInboxMessageBase> GeneralMessages { get; set; }

    }
}