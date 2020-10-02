using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IImageProcessingService
    {
        public Task ResizeImage(string filePath,  int size, out string outputPath);
        public bool TryResizeImage(string filePath,  int size, out string outputPath, out Task promise);
    }
}