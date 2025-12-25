using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("DefaultConnectionString"));
builder.Services.AddBusinessServices();

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789„·‡ÁÍÈËÌÌÔÛÛ„ı·‡·";

        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
    })
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.SlidingExpiration = true;
    options.LoginPath = "/LogIn/Index"; // Redirect here if not logged in
    options.LogoutPath = "/Home/Index"; // Logout page if the user logs out
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero; // Invalidate immediately 
});

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
