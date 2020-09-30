using System.Threading.Tasks;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IRecommendedDatabaseParser
    {
        Task StoreRecommendedToDatabase(Recommended recommended);
    }
}