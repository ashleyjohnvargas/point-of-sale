using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace POS1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ApplicationDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Login
        public IActionResult LoginPage()
        {
            return View("LoginPage");
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                _logger.LogError($"User not found: {email}");
                ViewBag.ErrorMessage = "Invalid email or password";
                return View("LoginPage");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning($"Inactive user login attempt: {email}");
                ViewBag.ErrorMessage = "Your account is inactive. Please contact support.";
                return View("LoginPage");
            }
           

            // Authentication using Cookies (Recommended over Session)
        //    var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Name, user.FullName)
        //};

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var authProperties = new AuthenticationProperties { IsPersistent = true };

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(claimsIdentity),
        //        authProperties
        //    );

            //return RedirectToAction("Index", "Dashboard");
        //}
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))  // Compare the entered password with the hashed password
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserFullName", user.FullName);
                HttpContext.Session.SetString("UserEmail", user.Email);

                // Update LastLogin timestamp
                user.LastLogin = DateTime.UtcNow;
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid login credentials. Please try again.";
            return View("LoginPage");
        }

        // Background Service for checking inactive users and setting their last login date

        public class InactiveUserChecker : BackgroundService
        {
            private readonly IServiceScopeFactory _scopeFactory;

            public InactiveUserChecker(IServiceScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var threshold = DateTime.UtcNow.AddMonths(-6);
                    var inactiveUsers = dbContext.Users.Where(u => u.LastLogin < threshold && u.IsActive).ToList();

                    foreach (var user in inactiveUsers)
                    {
                        user.IsActive = false;
                    }

                    await dbContext.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run daily
                }
            }
        }
    }
}

