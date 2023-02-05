using Microsoft.AspNetCore.Identity;
using SendGrid.Extensions.DependencyInjection;
using FreshFarmMarket.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using FreshFarmMarket.Services;
using AspNetCore.ReCaptcha;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = true; options.Lockout.AllowedForNewUsers = true; options.Lockout.MaxFailedAccessAttempts = 3; options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); })
        .AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
    Config.LogoutPath = "/Logout";
    Config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});
builder.Services.AddSendGrid(options =>
    options.ApiKey = "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"
                     ?? throw new Exception("The 'SendGridApiKey' is not configured")
);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/errors/{0}");


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

app.Run();
