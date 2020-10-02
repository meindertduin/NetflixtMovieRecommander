using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IImageProcessingService
    {
        public Task ProcessImage(string filePath,  int size, out string outputPath);
        public bool TryProcessImage(string filePath,  int size, out string outputPath, out Task promise);
    }
}