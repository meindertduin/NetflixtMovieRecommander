using System;
using System.ComponentModel.DataAnnotations;
using NetflixMovieRecommander.Models.Enums;

namespace NetflixMovieRecommander.Models
{
    public class InboxMessage
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        
        public string ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }

        public string SenderId { get; set; }
        public UserProfile Sender { get; set; }

        public DateTime DateSend { get; set; }
    }
}