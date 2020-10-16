using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetflixMoviesRecommender.api.Pages.Admin
{
    [Authorize(Policy = ApiConstants.Policies.Mod)]
    public class Index : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}