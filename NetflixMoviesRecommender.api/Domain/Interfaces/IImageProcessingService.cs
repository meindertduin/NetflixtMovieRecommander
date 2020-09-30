using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IImageProcessingService
    {
        public Task ProcessImage(string filePath, out string outputPath, int size);
    }
}