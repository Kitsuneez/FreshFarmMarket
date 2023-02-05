using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class LoginTwoStepModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly AuthDbContext _context;
		public LoginTwoStepModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, AuthDbContext context)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			_logger = logger;
			_context = context;
		}
		private readonly ILogger<LoginModel> _logger;
		[BindProperty]
		public string OTP { get; set; }
		public async void OnGet()
        {

        }
		public async Task<IActionResult> OnPostAsync(string email)
		{
			var Emailuser = await userManager.FindByEmailAsync(email);
			var test = await signInManager.TwoFactorSignInAsync("Email", OTP, false, false);
			Console.WriteLine(test.Succeeded);
			if (test.Succeeded)
			{
				HttpContext.Session.SetString("UserName", Emailuser.Email);
				await signInManager.SignInAsync(Emailuser, false);
				return RedirectToPage("/Index");
			}
			return Page();
		}
    }
}
