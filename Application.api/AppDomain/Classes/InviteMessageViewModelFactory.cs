using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.AppDomain.Interfaces;

namespace NetflixMoviesRecommender.api.Domain
{
    public class InviteMessageViewModelFactory : IMessageViewModelFactory
    {
        private readonly GeneralMessageViewModelFactory _generalMessageViewModelFactory;
        
        public InviteMessageViewModelFactory()
        {
            _generalMessageViewModelFactory = new GeneralMessageViewModelFactory();
        }
        
        public InboxMessageViewModel CreateModel(InboxMessage inboxMessage, UserProfile sender)
        {
            var message = _generalMessageViewModelFactory.CreateModel(inboxMessage, sender);
            var invite = (WatchGroupInviteMessage) inboxMessage;
            var appendix = new WatchGroupInviteViewModel
            {
                GroupId = invite.GroupId,
                GroupTitle = invite.GroupTitle,
            };
            message.Appendix = appendix;
            return message;
        }
    }
}