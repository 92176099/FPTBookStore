using FPTBookStore.Models;
using FPTBookStore.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using FPTBookStore.Repositories.Abstract;
using FPTBookStore.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FPTBookContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddCoreAdmin();
builder.Services.AddSession();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<FPTBookContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.ConfigureApplicationCookie(op => op.LoginPath = "/UserAuthentication/Login");
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Events = new CookieAuthenticationEvents
//    {
//        OnRedirectToLogin = redirectContext =>
//        {
//            // Area's own login page

//            const string area = "/Admin";

//            if (redirectContext.Request.Path.StartsWithSegments(area))
//            {
//                var uriBuilder = new UriBuilder(redirectContext.RedirectUri);

//                uriBuilder.Path = area + uriBuilder.Path;

//                redirectContext.RedirectUri = uriBuilder.ToString();
//            }

//            return Task.CompletedTask;
//        }
//    };

//});
builder.Services.ConfigureApplicationCookie(options => {
    // Cookie settings  
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.LoginPath = "/Account/UserLogin";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
    static bool IsAdminContext(RedirectContext<CookieAuthenticationOptions> context)
    {
        return context.Request.Path.StartsWithSegments("/admin");
    }
    options.Events.OnRedirectToLogin = context =>
    {
        if (IsAdminContext(context))
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/UserAuthentication/AdminLogin" + redirectPath.Query);
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }

        return Task.CompletedTask;
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
