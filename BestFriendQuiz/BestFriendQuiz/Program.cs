using BestFriendQuiz.Context;
using BestFriendQuiz.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<BFQContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(BFQContext)).GetName().Name);
    });
});


builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // Kullanýcý Giriþi gerekli
    config.Filters.Add(new AuthorizeFilter(policy)); // Authentike olmuþ bir kullanýcý gerekli demiþ olduk.
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Home/Index";
});
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<BFQContext>();



var app = builder.Build();

if( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
