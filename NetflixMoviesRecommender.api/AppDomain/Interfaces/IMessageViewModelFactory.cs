using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.AppDomain.Interfaces
{
    public interface IMessageViewModelFactory
    {
        public InboxMessageViewModel CreateModel(InboxMessage inboxMessage, UserProfile sender);
    }
}