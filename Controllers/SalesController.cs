using Microsoft.AspNetCore.Mvc;

namespace POS1.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Sales()
        {
            return View();
        }
    }
}
