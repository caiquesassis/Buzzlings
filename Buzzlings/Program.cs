using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
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
    options.Cookie.HttpOnly = true; //Prevents JS from reading the cookie. Blocks session theft via XSS.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.SlidingExpiration = true; //If user keeps interacting, refresh the token
    options.LoginPath = "/LogIn/Index"; //Redirect here if not logged in
    options.LogoutPath = "/Home/Index"; //Logout page if the user logs out
});

//Identity-specific configuration
//Forces the app to check the user's "Security Stamp" on every request.
//This ensures that if e.g. a password is changed, all other active sessions 
//across different devices/browsers are logged out IMMEDIATELY 
//rather than waiting for the default 30 minute background check.
//PROS: Security / CONS: Performance. In a big app, leave this as 5 - 15 minutes.
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero; //Invalidate immediately 
});

//Scans the code, finds controllers, generates a JSON that describes the API
builder.Services.AddOpenApi();

//Testing health checks on localhost can produce false positives due to SQL Server Shared Memory.
//To accurately simulate a failure without changing the connection string,
//the SQL Server Service must be manually stopped in Windows Services
builder.Services.AddHealthChecks()
    //AddDbContextCheck is a method provided by the Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore package
    //The "cancellationToken" variable here is passed in by the framework which acts as a Stop signal
    //The Health Check system has a default Timeout - once reached, the system invalidates the token
    //EF Core sees the token is not valid and immediately stops trying to connect to the database
    //The thread is released, and the /health page returns Unhealthy because it took too long.
    //CanConnectAsync attempts to open a connection to the database and immediately close it.
    .AddDbContextCheck<ApplicationDbContext>(
        //If we don't specify a name, it just uses our class name
        //However, for a person checking the status, they don't care about the class name,
        //they just want to know if the "database" is up.
        name: "database",
        customTestQuery: (context, cancellationToken) => context.Database.CanConnectAsync(cancellationToken)
    );

var app = builder.Build();

app.UseStaticFiles();

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

//We could use just app.MapHealthChecks("/health") but
//HealthCheckOptions provides us with advanced info if we need it
//And here we're returning a JSON instead of just a plain text response
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                component = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            duration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

//In a real-world production app, it'd be best to wrap this in if(app.Environment.IsDevelopment())
//I'm leaving it enabled here so the API can be interacted with in the live demo
app.MapOpenApi(); //Scans the code, finds controllers, generates a JSON that describes the API
app.MapScalarApiReference(); //Generates the interactive API UI based on the JSON generated above

app.Run();
