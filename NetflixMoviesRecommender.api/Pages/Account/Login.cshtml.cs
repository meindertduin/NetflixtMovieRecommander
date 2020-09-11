using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetflixMoviesRecommender.api.Pages.Account
{
    public class Login : PageModel
    {
        [BindProperty] public LoginForm Form { get; set; }

        
        public void OnGet(string returnUrl)
        {
            Form = new LoginForm() {ReturnUrl = returnUrl};
        }

        public async Task<IActionResult> OnPost([FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var signingResult = await signInManager.PasswordSignInAsync(Form.Username, Form.Password, true, false);

            if (signingResult.Succeeded)
            {
                return Redirect(Form.ReturnUrl);
            }

            return Page();
        }
        
        
    }

    public class LoginForm
    {
        [Required]
        public string ReturnUrl { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}