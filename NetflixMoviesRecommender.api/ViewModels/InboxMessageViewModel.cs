using System;
using NetflixMovieRecommander.Models.Enums;

namespace NetflixMovieRecommander.Models
{
    public class InboxMessageViewModel
    {
        public int MessageId { get; set; }
        public MessageType MessageType { get; set; }
        public string Title { get; set; }
        public Object Appendix { get; set; }
        public string Description { get; set; }
        public UserProfileViewModel Sender { get; set; }
        public DateTime DateSend { get; set; }
        
    }
}