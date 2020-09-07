using System.Collections.Generic;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IWatchListFileParserService
    {
        void StoreUserWatchlistItems(List<string> watchListFilePaths);
    }
}