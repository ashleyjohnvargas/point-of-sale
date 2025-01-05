using Microsoft.AspNetCore.Mvc;

namespace POS1.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
