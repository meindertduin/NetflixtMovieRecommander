using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.Pages.Account
{
    public class Register : PageModel
    {
        [BindProperty] public RegisterForm Form { get; set; }
        
        public void OnGet(string returnUrl)
        {
            Form = new RegisterForm() { ReturnUrl = returnUrl };
        }

        public async Task<IActionResult> OnPost(
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] SignInManager<ApplicationUser> signInManager)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            
            var user = new ApplicationUser(Form.Username) { Email = Form.Email };
            
            user.UserProfile = new UserProfile
            {
                UserName = Form.Username,
            };
            
            var creationResult = await userManager.CreateAsync(user, Form.Password);

            if (creationResult.Succeeded)
            {
                await signInManager.SignInAsync(user, true);

                return Redirect(Form.ReturnUrl);
            }

            return Page();
        }
    }

    public class RegisterForm
    {
        [Required] public string ReturnUrl { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords dont match")]
        public string ConfirmPassword { get; set; }
        
    }
}