using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixMovieRecommander.Data;
using NetflixMoviesRecommender.api.Services;

namespace NetflixMoviesRecommender.api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));

            services.AddScoped<IWatchListFileParserService, WatchListFileParserService>();

            services.AddHttpClient();
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowClientOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                });
            });
            
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
            
            app.UseRouting();

            app.UseCors("AllowClientOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}