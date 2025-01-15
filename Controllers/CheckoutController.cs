// Controllers/CheckoutController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using System.Linq;

namespace POS1.Controllers
{
    public class CheckoutController : Controller
    {
		private readonly ApplicationDbContext _context;

		public CheckoutController(ApplicationDbContext context)
		{
			_context = context;
		}


		[HttpGet]
		public async Task<IActionResult> Checkout(int id) // id is the OrderId
		{
			// Fetch the order, customer, and items based on the OrderId
			var order = await _context.Orders
				.Include(o => o.Customer) // Include related customer data
				.Include(o => o.OrderItems) // Include related order items
				.ThenInclude(oi => oi.Product) // Include product details in order items
				.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
			{
				return NotFound(); // Return 404 if order not found
			}

			// Prepare the CheckoutViewModel
			var checkoutViewModel = new CheckoutViewModel
			{
				CustomerName = _context.Customers
                        .Where(c => c.EcomId == order.CustomerId) // Match EcomId with CustomerId
                        .Select(c => c.CustomerName)
                        .FirstOrDefault() ?? "Guest",  // Safeguard for null values
                OrderId = order.OrderId,
				TotalAmount = order.TotalPrice,
				PaymentMethod = order.PaymentMethod,
				OrderItems = order.OrderItems.Select(oi => new CheckoutItemViewModel
				{
					ProductName = oi.Product.Name,
					Price = oi.Product.Price,
					Quantity = oi.Quantity,
					Subtotal = oi.Subtotal
				}).ToList()
			};

			// Pass the model to the view
			return View(checkoutViewModel);
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

        //[HttpPost]
        //public IActionResult Checkout(CheckoutModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Logic to process checkout (e.g., payment gateway, database update)
        //        // For now, we simulate success
        //        return RedirectToAction("Confirmation");
        //    }

        //    return View("Index", model);
        //}

        public IActionResult Confirmation()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Checkout(List<CartItem> cartItems, string customerName, string paymentMethod, decimal paymentAmount)
        //{
        //    if (cartItems == null || !cartItems.Any())
        //    {
        //        ModelState.AddModelError("", "The cart is empty.");
        //        return View("Checkout");
        //    }

        //    // Calculate total amount
        //    decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        //    // Calculate change
        //    decimal changeAmount = paymentAmount - totalAmount;

        //    if (changeAmount < 0)
        //    {
        //        ModelState.AddModelError("", "Insufficient payment. Please provide enough payment.");
        //        return View("Checkout");
        //    }

            //// Generate invoice
            //var invoice = new Invoice
            //{
            //    CustomerName = customerName,
            //    PaymentMethod = paymentMethod,
            //    PaymentAmount = paymentAmount,
            //    TotalAmount = totalAmount,
            //    ChangeAmount = changeAmount,
            //    DateCreated = DateTime.Now
            //};

            //// Save to database
            //_context.Invoices.Add(invoice);
            //_context.SaveChanges();

            //// Redirect to Invoice page or show confirmation
            //return RedirectToAction("InvoiceDetails", new { id = invoice.InvoiceNumber });
        }

    }
