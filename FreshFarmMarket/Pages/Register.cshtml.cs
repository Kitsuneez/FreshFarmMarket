using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using FreshFarmMarket.ViewModels;

namespace FreshFarmMarket.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private RoleManager<IdentityRole> roleManager { get; }
        private EmailSender _emailsender;
        private readonly AuthDbContext _authDbContext;

        private IWebHostEnvironment _environment;
        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IWebHostEnvironment environment,
        EmailSender EmailSender,
        AuthDbContext authDbContext,
        RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            _emailsender = EmailSender;
            _authDbContext = authDbContext;
            this.roleManager = roleManager;

        }

        [BindProperty]
        public Register RModel { get; set; } = new Register();
        [BindProperty]
        public IFormFile? Upload { get; set; }
        public string[] Genders = new[] { "Male", "Female" }; 
        public void OnGet(string? email, string? pfp)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                RModel.Email = email;

            }
            if (!string.IsNullOrWhiteSpace(pfp))
            {
                RModel.Photo = pfp;

            }
        }


        public async Task<IActionResult> OnPostAsync(string? pfp)
        {
			if (ModelState.IsValid)
            {
                var Checkuser = await userManager.FindByEmailAsync(RModel.Email);
                if(Checkuser != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("{0} already exist",
                    Checkuser);
                    return Page();
                }
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var user = new ApplicationUser()
                {
                    UserName = @HtmlEncoder.Default.Encode(RModel.Email),
                    Email = @HtmlEncoder.Default.Encode(RModel.Email),
                    CreditCard = protector.Protect(RModel.CreditCard),
                    FirstName= @HtmlEncoder.Default.Encode(RModel.FirstName),
                    LastName= @HtmlEncoder.Default.Encode(RModel.LastName),
                    Gender = RModel.Gender,
                    Address = @HtmlEncoder.Default.Encode(RModel.Address),
                    AboutMe= @HtmlEncoder.Default.Encode(RModel.AboutMe),
                    PhoneNumber = RModel.PhoneNumber,
                    TwoFactorEnabled = true
                };
                if (Upload != null)
                {
                    if (Upload.Length > 20 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 20MB.");
                        return Page();
                    }
                    var uploadsFolder = "Uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    user.Photo = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                    }
                if(Upload == null) {
                    user.Photo = pfp;
                }
                var result = await userManager.CreateAsync(user, RModel.Password);
                await userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmation = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
                    await _emailsender.Execute("Account Verfication", confirmation!, user.Email);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("Email has been sent for verification");
                    return Redirect("/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }







    }
}
