using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.AppDomain.Interfaces;

namespace NetflixMoviesRecommender.api.Domain
{
    public class GeneralMessageViewModelFactory : IMessageViewModelFactory
    {
        public InboxMessageViewModel CreateModel(InboxMessage inboxMessage, UserProfile sender)
        {
            return new InboxMessageViewModel
            {
                MessageId = inboxMessage.Id,
                MessageType = inboxMessage.MessageType,
                Title = inboxMessage.Title,
                Description = inboxMessage.Description,
                Sender = new UserProfileViewModel
                {
                    UserName = sender.UserName,
                    Id = sender.Id,
                    AvatarUrl = AppHttpContext.AppBaseUrl + "/api/profile/picture/" + sender.Id,
                },
                DateSend = inboxMessage.DateSend,
            };
        }
    }
}