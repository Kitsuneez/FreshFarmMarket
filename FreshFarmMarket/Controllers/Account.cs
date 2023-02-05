using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FreshFarmMarket.Model;
using FreshFarmMarket.Services;

namespace FreshFarmMarket.Controllers
{
    public class Account : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; }

        private IWebHostEnvironment _environment;
        private EmailSender _emailsender;
        private SignInManager<ApplicationUser> signInManager { get; }

        public Account(IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailSender emailsender)
        {
            _environment = environment;
            _userManager = userManager;
            this.signInManager = signInManager;
            _emailsender = emailsender;
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
                if (user == null || token == null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Invalid Email");
                    return Redirect("/Error");
                }

            var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("User {0} have been confirmed", user.UserName);
				    return Redirect("/");
                }
            TempData["FlashMessage.Type"] = "danger";
            TempData["FlashMessage.Text"] = string.Format("Invalid Email");
            return View();
        }
    }
}
