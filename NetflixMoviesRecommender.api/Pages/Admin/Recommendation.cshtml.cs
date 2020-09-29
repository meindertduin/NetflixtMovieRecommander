using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;

namespace NetflixMoviesRecommender.api.Pages.Admin
{
    public class Recommendation : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRecommendedDatabaseParser _recommendedDatabaseParser;
        private readonly AppDbContext _ctx;
        [BindProperty] public string Title { get; set; }
        [BindProperty] public List<NetflixRecommended> Recommendations { get; set; }
        
        public Recommendation(IConfiguration configuration, IRecommendedDatabaseParser recommendedDatabaseParser, AppDbContext ctx)
        {
            _configuration = configuration;
            _recommendedDatabaseParser = recommendedDatabaseParser;
            _ctx = ctx;
        }
        
        public IActionResult OnGet()
        {
            Recommendations = _ctx.NetflixRecommendations.OrderBy(x => x.Title).ToList();
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
                await _recommendedDatabaseParser.StoreRecommendedToDatabase(recommended);
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