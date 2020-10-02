using System;
using System.IO;
using System.Linq;
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
        
        public Task ResizeImage(string filePath, int size, out string outputPath)
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
                    
                    var savePath = Path.Combine(_env.WebRootPath, string.Concat(Path.GetRandomFileName(), ".", filePath.Split('.').Last()));
                    outputPath = savePath;
                    return image.SaveAsync(savePath);
                }
            }
        }

        public bool TryResizeImage(string filePath, int size, out string outputPath, out Task promise)
        {
            try
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

                        var savePath = Path.Combine(_env.WebRootPath,
                            string.Concat(Path.GetRandomFileName(), ".", filePath.Split('.').Last()));
                        outputPath = savePath;
                        promise = image.SaveAsync(savePath);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                outputPath = null;
                promise = null;
                
                return false;
            }
        }
    }
}