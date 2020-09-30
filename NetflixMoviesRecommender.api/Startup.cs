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
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Domain;
using NetflixMoviesRecommender.api.Domain.Extensions;
using NetflixMoviesRecommender.api.Domain.Services;
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
            AddIdentity(services);
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("AppDbContext"), b => b.MigrationsAssembly("NetflixMoviesRecommender.api")));



            services.AddTransient<IRecommendedDatabaseParser, RecommendedDatabaseParser>();
            services.AddTransient<IFileHandlerService, FileHandlerService>();
            services.AddTransient<IImageProcessingService, ImageProcessingService>();

            services.AddHttpClient();
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowClientOrigin", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpContext();
            
            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
            });
            
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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


                services.ConfigureApplicationCookie(config =>
                {
                    config.LoginPath = "/account/login";
                    config.LogoutPath = "/api/auth/logout";
                });
                
            var identityServiceBuilder = services.AddIdentityServer();

            identityServiceBuilder.AddAspNetIdentity<ApplicationUser>();
            
            var assembly = typeof(Startup).Assembly.GetName().Name;

            if (_env.IsDevelopment())
            {
                identityServiceBuilder
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(_configuration.GetConnectionString("AppDbContext"),
                            sql => sql.MigrationsAssembly(assembly));
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(_configuration.GetConnectionString("AppDbContext"),
                            sql => sql.MigrationsAssembly(assembly));
                    })
                    .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                    .AddInMemoryClients(IdentityConfig.GetClients())
                    .AddInMemoryApiScopes(IdentityConfig.GetApiScopes());

                identityServiceBuilder.AddDeveloperSigningCredential();
            }

            services.AddLocalApiAuthentication();

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "server";
                    config.LoginPath = "/account/login";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy(ApiConstants.Policies.Mod, policy =>
                {
                    var isPolicy = options.DefaultPolicy;
                    policy.Combine(isPolicy);
                    policy.RequireClaim(ApiConstants.Claims.Role, ApiConstants.Roles.Mod);
                    policy.RequireAuthenticatedUser();
                });
            });
        }
    }

    public struct ApiConstants
    {
        public struct Policies
        {
            public const string Mod = nameof(Mod);
        }
        
        public struct Claims
        {
            public const string Role = nameof(Role);
        }
        
        public struct Roles
        {
            public const string Mod = nameof(Mod);
        }
    }
}