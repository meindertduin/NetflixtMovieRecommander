using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NetflixMoviesRecommender.api.Services;

namespace NetflixMoviesRecommender.api.Domain
{
    public class FileHandlerService : IFileHandlerService
    {
        private readonly IWebHostEnvironment _env;
        private const int DEFAULT_MAX_FILE_SIZE = 100_000_000;

        public FileHandlerService(IWebHostEnvironment env)
        {
            _env = env;
        }
        
        public async Task<string> SaveFile(IFormFile file, int max_size=DEFAULT_MAX_FILE_SIZE)
        {
            if (file.Length > max_size)
            {
                return null;
            }
            
            var mime = file.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(fileStream);

            return savePath;
        }
        
        
        public async Task<string> SaveFile(IFormFile file, string[] allowedFileExtensions, int maxSize=DEFAULT_MAX_FILE_SIZE)
        {
            var extension = Path.GetExtension(file.FileName);
            if (allowedFileExtensions.Contains(extension.ToLower()) == false)
            {
                return null;
            }
            
            var savePath = await SaveFile(file, maxSize);

            return savePath;
        }
    }
}