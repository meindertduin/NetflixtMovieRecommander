using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.Pages.Account
{
    [Authorize]
    public class ChangePassword : PageModel
    {
        [BindProperty] public PasswordChangeForm PasswordChangeForm { get; set; }
        
        public void OnGet(string returnUrl)
        {
            PasswordChangeForm = new PasswordChangeForm() {ReturnUrl = returnUrl}; 
        }

        public async Task<IActionResult> OnPost([FromServices] UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var result = await userManager.ChangePasswordAsync(user ,PasswordChangeForm.Password,
                PasswordChangeForm.NewPassword);

            if (result.Succeeded)
            {
                return Redirect(PasswordChangeForm.ReturnUrl);
            }
            
            return Page();
        }
    }

    public class PasswordChangeForm
    {
        [Required] 
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New passwords dont match")]
        public string ConfirmNewPassword { get; set; }

        public string ReturnUrl { get; set; }
        
    }
}