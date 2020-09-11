using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NetflixMovieRecommender;Integrated Security=True; MultipleActiveResultSets=True";

            AddIdentity(services);
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conn, b => b.MigrationsAssembly("NetflixMoviesRecommender.api")));



            services.AddTransient<IWatchListFileParserService, WatchListFileParserService>();
            services.AddTransient<IRecommendedDatabaseParser, RecommendedDatabaseParser>();

            services.AddHttpClient();
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowClientOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                    builder.AllowAnyHeader();
                    builder.AllowAnyHeader();
                });
            });
            
            services.AddControllers();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
            app.UseCors("AllowClientOrigin");
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseIdentityServer();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }

        private void AddIdentity(IServiceCollection services)
        {
            string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NetflixMovieRecommender;Integrated Security=True; MultipleActiveResultSets=True";


            services.AddDbContext<AppIdentityDbContext>(config =>
                //config.UseInMemoryDatabase("Dev");
                config.UseSqlServer(conn, b => b.MigrationsAssembly("NetflixMoviesRecommender.api")));

                services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    if (_env.IsDevelopment())
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                    else
                    {
                        // add in configs for production
                    }
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            var identityServiceBuilder = services.AddIdentityServer();

            identityServiceBuilder.AddAspNetIdentity<IdentityUser>();
            
            var assembly = typeof(Startup).Assembly.GetName().Name;

            if (_env.IsDevelopment())
            {
                identityServiceBuilder
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(conn,
                            sql => sql.MigrationsAssembly(assembly));
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(conn,
                            sql => sql.MigrationsAssembly(assembly));
                    })
                    .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                    .AddInMemoryClients(IdentityConfig.GetClients())
                    .AddInMemoryApiScopes(IdentityConfig.GetApiScopes());

                identityServiceBuilder.AddDeveloperSigningCredential();
            }

            services.AddLocalApiAuthentication();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Mod", policy =>
                {
                    var isPolicy = options.GetPolicy(IdentityServerConstants.LocalApi.PolicyName);
                    policy.Combine(isPolicy);
                    policy.RequireClaim("role", "Mod");
                });
            });
        }
    }
}