using BarTicaret.Infrastructure;
using BarTicaret.Application.Products;
using BarTicaret.WebUI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using BarTicaret.Application.Lookup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Db + repo + application servisleri
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ILookupService, LookupService>();


// Basit demo kimlik
builder.Services.AddSingleton<IAuthService, SimpleAuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.LogoutPath = "/Auth/Logout";
        opt.AccessDeniedPath = "/Auth/Denied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}");

app.Run();
