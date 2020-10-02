using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Services
{
    public interface IFileHandlerService
    {
        Task<string> SaveFile(IFormFile file, int maxSize);
        Task<string> SaveFile(IFormFile file, string[] allowedFileExtensions, int maxSize);

    }
}