using Microsoft.AspNetCore.Mvc;
using POS1.Models;


namespace POS1.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Users()
        {
            return View();
        }
    }
}
