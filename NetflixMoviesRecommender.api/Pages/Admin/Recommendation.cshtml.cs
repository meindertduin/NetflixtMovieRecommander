using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetflixMovieRecommander.Models;
using NetflixMoviesRecommender.api.Services;
using Newtonsoft.Json;

namespace NetflixMoviesRecommender.api.Pages.Admin
{
    public class Recommendation : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRecommendedDatabaseParser _recommendedDatabaseParser;
        [BindProperty] public string Title { get; set; }

        public Recommendation(IConfiguration configuration, IRecommendedDatabaseParser recommendedDatabaseParser)
        {
            _configuration = configuration;
            _recommendedDatabaseParser = recommendedDatabaseParser;
        }
        
        public IActionResult OnGet()
        {
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
            }
            catch (Exception e)
            {
                // ignored
            }

            return Page();
        }
        
    }
}