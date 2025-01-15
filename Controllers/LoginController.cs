using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace POS1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult LoginPage()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserFullName", user.FullName);
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid email or password. Please check your credentials or contact your administrator";
            return View("LoginPage");
        }

        public IActionResult RegisterPage()
        {
            return View(); // Return the view for the register page
        }
    }
}
       
