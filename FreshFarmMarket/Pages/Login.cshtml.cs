using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using AspNetCore.ReCaptcha;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace FreshFarmMarket.Pages
{
	[ValidateReCaptcha]
	public class LoginModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly AuthDbContext _context;
		public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, AuthDbContext context)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			_logger = logger;
			_context = context;
		}
		private readonly ILogger<LoginModel> _logger;
		[BindProperty]
		public Login LModel { get; set; }
		public AuditLog AModel { get; set; } = new AuditLog();

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);
				if (identityResult.Succeeded)
				{
                    HttpContext.Session.SetString("UserName", LModel.Email);
					var user = await userManager.FindByEmailAsync(LModel.Email);
					var userId = await userManager.GetUserIdAsync(user);
					if(userId != null)
					{
						AModel.userId = userId;
						AModel.action = "Logged In";
						AModel.timeStamp = DateTime.Now;
						_context.AuditLogs.Add(AModel);
						_context.SaveChanges();
					}
					return RedirectToPage("/Index");
				}
                if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "The account is locked out");
					TempData["FlashMessage.Text"] = "Account is locked out";
					TempData["FlashMessage.Type"] = "error";
					return Page();
                }
                TempData["FlashMessage.Text"] = "username or password incorrect";
                TempData["FlashMessage.Type"] = "error";
                ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}
