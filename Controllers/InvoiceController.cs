using Microsoft.AspNetCore.Mvc;

namespace POS1.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
