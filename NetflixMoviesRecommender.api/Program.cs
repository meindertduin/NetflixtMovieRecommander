using System.Linq;
using System.Security.Claims;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                
                var testUser = new ApplicationUser("test"){ Email = "test@test.com"};
                userManager.CreateAsync(testUser, "password").GetAwaiter().GetResult();

                var mod = new ApplicationUser("mod"){ Email = "mod@mod.com"};
                userManager.CreateAsync(mod, "password").GetAwaiter().GetResult();
                userManager.AddClaimAsync(mod, new Claim(ApiConstants.Claims.Role, ApiConstants.Roles.Mod)).GetAwaiter().GetResult();
                
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityConfig.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityConfig.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                
                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in IdentityConfig.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}