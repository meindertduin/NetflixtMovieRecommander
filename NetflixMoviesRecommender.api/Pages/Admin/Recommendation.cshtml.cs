using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;

namespace NetflixMoviesRecommender.api.Pages.Admin
{
    [Authorize(Policy = ApiConstants.Policies.Mod)]
    public class Recommendation : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _ctx;
        [BindProperty] public string Title { get; set; }
        [BindProperty] public List<NetflixRecommended> Recommendations { get; set; }
        
        [BindProperty(SupportsGet = true)] public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)] public int DisplayPerPage { get; set; } = 50;
        [BindProperty(SupportsGet = true)] public double PagesCount { get; set; }

        public Recommendation(IConfiguration configuration, AppDbContext ctx)
        {
            _configuration = configuration;
            _ctx = ctx;
        }
        
        public IActionResult OnGet(int index)
        {
            int skipAmount = DisplayPerPage * index;
            
            if (string.IsNullOrEmpty(SearchString) == false)
            {
                Recommendations = _ctx.NetflixRecommendations
                    .OrderBy(x => x.Title)
                    .Where(x => x.Title.Contains(SearchString))
                    .Skip(skipAmount)
                    .Take(DisplayPerPage)
                    .ToList();
            }
            else
            {
                Recommendations = _ctx.NetflixRecommendations
                    .OrderBy(x => x.Title)
                    .Skip(skipAmount)
                    .Take(DisplayPerPage)
                    .ToList();
            }

            PagesCount = Math.Ceiling( _ctx.NetflixRecommendations.ToArray().Length / (double) DisplayPerPage);
            
            return Page();
        }

        public async Task<IActionResult> OnPost(
            [FromServices] IHttpClientFactory clientFactory)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            
            var client = clientFactory.CreateClient();

            var res = await client.GetAsync($"http://www.omdbapi.com/" +
                                            $"?apikey={_configuration.GetValue<string>("OMDB_KEY")}" +
                                            $"&t={Title}");

            var resJsonString = await res.Content.ReadAsStringAsync();
            var recommended = JsonConvert.DeserializeObject<Recommended>(resJsonString);
            
            if (string.IsNullOrEmpty(recommended.Title))
            {
                return Page();
            }

            try
            {
                var existingEntity = _ctx.NetflixRecommendations
                    .FirstOrDefault(x => x.Title == recommended.Title);

                if (existingEntity == null)
                {
                    NetflixRecommended netflixRecommended = new NetflixRecommended
                    {
                        Title = recommended.Title.Split(':')[0],
                        Plot = recommended.Plot,
                        Poster = recommended.Poster,
                        Type = recommended.Type,
                        Genres = recommended.Genre,
                    };
                
                    await _ctx.NetflixRecommendations.AddRangeAsync(netflixRecommended);
                    await _ctx.SaveChangesAsync();
                }
                return RedirectToPage();
            }
            
            catch (Exception e)
            {
                // ignored
            }
    
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var recommendation = _ctx.NetflixRecommendations.FirstOrDefault(x => x.Id == id);
            if (recommendation != null)
            {
                _ctx.NetflixRecommendations.Remove(recommendation);
                _ctx.SaveChanges();
            }
            
            return RedirectToPage();
        }
        
    }
}