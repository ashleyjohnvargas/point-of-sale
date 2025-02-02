using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS1.Controllers
{
    public class InvoiceController : Controller
    {
       // [Authorize]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            return View();
        }
    }
}
