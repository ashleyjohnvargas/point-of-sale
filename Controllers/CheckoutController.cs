// Controllers/CheckoutController.cs
using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using System.Linq;

namespace POS1.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Index()
        {
            // Example: This would ideally come from the user's session or database
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductName = "Product 1", Price = 10.00m, Quantity = 2 },
                new CartItem { ProductName = "Product 2", Price = 15.00m, Quantity = 1 }
            };

            var checkoutModel = new CheckoutModel
            {
                CartItems = cartItems,
                Subtotal = cartItems.Sum(item => item.Subtotal),
                Total = cartItems.Sum(item => item.Subtotal) + 5.00m // Example shipping fee
            };

            return View(checkoutModel);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to process checkout (e.g., payment gateway, database update)
                // For now, we simulate success
                return RedirectToAction("Confirmation");
            }

            return View("Index", model);
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
