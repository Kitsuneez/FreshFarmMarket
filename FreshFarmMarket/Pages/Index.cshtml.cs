using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using FreshFarmMarket.Model;

namespace FreshFarmMarket.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            this.userManager = UserManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                await signInManager.SignOutAsync();
                return RedirectToPage("Login");
            }
            //return StatusCode(StatusCodes.Status403Forbidden);
            return Page();
        }
    }
}