using POS1.Models;
using Microsoft.EntityFrameworkCore;
using POS1.Services;
using static POS1.Controllers.LoginController;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//// Ecommerce service
//builder.Services.AddHttpClient<EcommerceService>(client =>
//{
//    client.BaseAddress = new Uri("https://gizmodeecommerce.azurewebsites.net/"); // Replace with Ecommerce System URL
//});


//// Inventory service
//builder.Services.AddHttpClient<InventoryService>(client =>
//{
//    client.BaseAddress = new Uri("https://gizmodeinventorysystem2.azurewebsites.net/"); // Replace with Ecommerce System URL
//});

// Ecommerce service Local
builder.Services.AddHttpClient<EcommerceService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44385/"); // Replace with Ecommerce System URL
});


// Inventory service Local
builder.Services.AddHttpClient<InventoryService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44341/"); // Replace with Ecommerce System URL
});


// Add services to the container.
// Add Localizatiom - PH
builder.Services.AddLocalization();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEcommerceSystem", policy =>
    {
        policy.WithOrigins("https://localhost:44385", "https://gizmodeecommerce.azurewebsites.net/") // Replace with your actual front-end URLs
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Add background service for checking inactive users
builder.Services.AddHostedService<InactiveUserChecker>();


// configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login/LoginPage"; // Redirect to login if unauthorized
            options.LogoutPath = "/Logout";   // Redirect when logging out
            options.AccessDeniedPath = "/Login/AccessDenied"; // Handle access denied scenario
        });



// Add session services to the container
builder.Services.AddDistributedMemoryCache(); // Adds in-memory caching for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout as per your requirement
    options.Cookie.HttpOnly = true; // Only accessible by the server
    options.Cookie.IsEssential = true; // Necessary for session management
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}"); // Handle 404 page
    app.UseHsts();
}
app.UseRequestLocalization("en-PH");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();//
app.UseCors("AllowEcommerceSystem");

// Add session middleware to the request pipeline
app.UseSession(); // This should come before UseAuthorization
app.UseAuthentication(); // This should be placed before UseAuthorization
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"); 

//// THIS IS THE ORIGINAL, BRING BACK THIS:
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=LoginPage}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Sales}/{action=Sales}");



app.Run();
