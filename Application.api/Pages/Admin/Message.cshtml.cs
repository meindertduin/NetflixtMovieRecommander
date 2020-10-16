using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;
using NetflixMovieRecommander.Models.Enums;

namespace NetflixMoviesRecommender.api.Pages.Admin
{
    [Authorize(Policy = ApiConstants.Policies.Mod)]
    public class Message : PageModel
    {
        [BindProperty(SupportsGet = true)] public GlobalMessageModel GlobalMessage { get; set; }
        
        public void OnGet()
        {
            
        }

        public IActionResult OnPostMessage([FromServices] AppDbContext ctx)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            foreach (var ctxUserProfile in ctx.UserProfiles.AsNoTracking())
            {
                try
                {
                    ctx.InboxMessages.Add(new InboxMessage
                    {
                        MessageType = GlobalMessage.MessageType,
                        Title = GlobalMessage.Title,
                        Description = GlobalMessage.Description,
                        SenderId = "8b39233c-96a4-43c6-b656-8e5662daabdf",
                        ReceiverId = ctxUserProfile.Id,
                        DateSend = DateTime.Now,
                    });
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            ctx.SaveChanges();
            
            return RedirectToPage();
        }
    }

    public class GlobalMessageModel
    {
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(MessageType))] public MessageType MessageType { get; set; }
    }
}