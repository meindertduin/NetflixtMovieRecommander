using System;
using System.Linq;
using System.Threading.Tasks;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.Services
{
    public class RecommendedDatabaseParser : IRecommendedDatabaseParser
    {
        private readonly AppDbContext _ctx;

        public RecommendedDatabaseParser(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task StoreRecommendedToDatabase(Recommended recommended)
        {
            var existingEntity = _ctx.NetflixRecommendations
                .FirstOrDefault(x => x.Title == recommended.Title);

            if (existingEntity == null)
            {
                NetflixRecommended netflixRecommended = new NetflixRecommended
                {
                    Title = recommended.Title.Split(':')[0],
                    Plot = recommended.Plot,
                    Poster = recommended.Poster,
                    Type = recommended.Type,
                    Genres = recommended.Genre,
                };
                
                await _ctx.NetflixRecommendations.AddRangeAsync(netflixRecommended);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}