using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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

builder.Services.AddOpenApi();

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

//This is required for [Route] attributes to work when not using MapControllerRoute
//In recent versions of ASP.NET Core, when we call app.MapControllerRoute (like we did above)
//this already maps controllers so we don't need this line to use the API, for instance.
//However, adding this separately is a good way to define explicit intent, saying like:
//"this app explicitly supports REST API attribute routing".
//Besides, if I ever removed MVC Views (because MapControllerRoute is an MVC thing) and
//wanted to turn this into a pure API project, the API routes would break without this.
app.MapControllers();

//In a real-world production app, it'd be best to wrap this in if(app.Environment.IsDevelopment())
//I'm leaving it enabled here so the API can be interacted with in the live demo
app.MapOpenApi(); //Scans the code, finds controllers, generates a JSON that describes the API
app.MapScalarApiReference(); //Generates the interactive API UI based on the JSON generated above

app.Run();
