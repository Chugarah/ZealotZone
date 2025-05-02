
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using ZealotZone.Interfaces;
using ZealotZone.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MVC add this Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Let's add support for Entity Framework Core and connect to a database
var connectionString = builder.Configuration.GetConnectionString("SQLServer");
builder.Services.AddDbContext<DataContext>(
    d => d.UseAzureSql(connectionString)
    );

// Inspired by Hands, Configuring requirements and setting regarding registration
builder.Services.AddIdentity<UserEntity, IdentityRole>(
    i =>
    {
        i.User.RequireUniqueEmail = true;
        i.Password.RequiredLength = 4;
    });
// Yummi Cookies
builder.Services.ConfigureApplicationCookie(c =>
{
    c.LoginPath = "/user-login";
    c.LogoutPath = "/user-login";
    c.AccessDeniedPath = "/user-denied";
    c.Cookie.Name = "ZealotZone.Cookie";
    c.Cookie.IsEssential = true;
    c.Cookie.HttpOnly = true;
    c.Cookie.SameSite = SameSiteMode.Strict;
    c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    c.ExpireTimeSpan = TimeSpan.FromDays(7);
    c.SlidingExpiration = true;
});



// Changing Razor Engine to look for Features location
// Changing to Features' Folder Inspiration
// https://softwareengineering.stackexchange.com/questions/338597/folder-by-type-or-folder-by-feature
// https://www.reddit.com/r/dotnet/comments/124to6h/project_architecture_do_you_organize_by_features/
// https://scottsauber.com/2016/04/25/feature-folder-structure-in-asp-net-core/
// https://code-maze.com/vertical-slice-architecture-aspnet-core/
// I did make this decision based on what AI recommended me for best practice,
// and also I like the design towards microservices.
// Refactored by Google Gemini Pro AI
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    // --- Advanced Routing that supports the Features folder and with Areas ---
    // Got help from Google Gemini to Refactor and improve this code
    // to support this custom routing :)
    // {2} - Area Name (e.g., "User")
    // {1} - Controller Name (e.g., "Login")
    // {0} - Action Name (e.g., "Index")
    options.AreaViewLocationFormats.Clear();
    // Look in Features/AreaName/ControllerName/ActionName.cshtml
    options.AreaViewLocationFormats.Add("/Features/{2}/{1}/{0}.cshtml");
    // Look in Features/AreaName/Shared/ActionName.cshtml
    options.AreaViewLocationFormats.Add("/Features/{2}/Shared/{0}.cshtml");
    // Look in Features/Shared/ActionName.cshtml
    options.AreaViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
    // Look in the standard shared location
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

    // --- Non-Area View Locations ---
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

    // --- Editor/Display Template Locations (Example for Areas) ---
    options.AreaViewLocationFormats.Add("/Features/{2}/{1}/EditorTemplates/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Features/{2}/Shared/EditorTemplates/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Features/Shared/EditorTemplates/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/Shared/EditorTemplates/{0}.cshtml");

    // --- Editor/Display Template Locations (Example for Non-Areas) ---
    options.ViewLocationFormats.Add("/Features/{1}/EditorTemplates/{0}.cshtml");
    options.ViewLocationFormats.Add("/Features/Shared/EditorTemplates/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Shared/EditorTemplates/{0}.cshtml");
});

// Lets register our DI containers
builder.Services.AddScoped<IDateService, DateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // This will automatically migrate when starting the application
    // https://stackoverflow.com/questions/65389260/what-app-usemigrationsendpoint-does-in-net-core-web-application-startup-class
    // app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Features/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// We can protect our pages with these directives
app.UseAuthentication();
app.UseAuthorization();

// Pointing to Log in Form
app.MapControllerRoute(
    name: "User", // Or "AreaRoute" or similar
    pattern: "{area:exists}/{controller=UserLogin}/{action=Index}"
);
app.Run();
