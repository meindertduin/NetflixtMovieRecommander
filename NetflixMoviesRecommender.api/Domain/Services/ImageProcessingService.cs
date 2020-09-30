using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using NetflixMoviesRecommender.api.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NetflixMoviesRecommender.api.Domain.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IWebHostEnvironment _env;

        public ImageProcessingService(IWebHostEnvironment env)
        {
            _env = env;
        }
        
        public Task ProcessImage(string filePath, out string outputPath, int size)
        {
            using (var input = File.OpenRead(filePath))
            {
                using (var image = Image.Load(input))
                {
                    image.Mutate(x =>
                    {
                        x.Resize(new ResizeOptions
                        {
                            Size = new Size(size, size)
                        });
                    });
                    
                    var savePath = Path.Combine(_env.WebRootPath, Path.GetRandomFileName());
                    outputPath = savePath;
                    return image.SaveAsync(savePath);
                }
            }
        }
    }
}