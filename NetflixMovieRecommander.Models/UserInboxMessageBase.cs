using System;

namespace NetflixMovieRecommander.Models
{
    public class UserInboxMessageBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeSend { get; set; }
        
        public string ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }

        public string SenderId { get; set; }
        public UserProfile Sender { get; set; }

        public int UserInboxId { get; set; }
        public UserInbox UserInbox { get; set; }
    }
}