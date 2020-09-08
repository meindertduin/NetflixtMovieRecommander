using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixMovieRecommander.Data;
using NetflixMoviesRecommender.api.Services;

namespace NetflixMoviesRecommender.api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NetflixMovieRecommender;Integrated Security=True; MultipleActiveResultSets=True";

            
            
            services.AddDbContext<AppDbContext>(options =>
                //options.UseInMemoryDatabase("Dev"));
                options.UseSqlServer(conn, b => b.MigrationsAssembly("NetflixMoviesRecommender.api")));

            services.AddScoped<IWatchListFileParserService, WatchListFileParserService>();
            services.AddScoped<IRecommendedDatabaseParser, RecommendedDatabaseParser>();

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