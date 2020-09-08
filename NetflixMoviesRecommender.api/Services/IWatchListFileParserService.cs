using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IWatchListFileParserService
    {
        Task StoreUserWatchlistItems(List<string> watchListFilePaths);
    }
}