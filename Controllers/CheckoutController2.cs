// Controllers/CheckoutController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using POS1.Services;
using System.Linq;


namespace POS1.Controllers
{
    public class CheckoutController2 : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController2(ApplicationDbContext context)
        {
            _context = context;
        }
        // [Authorize]
        public IActionResult BlankCheckout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            return View();
        }

        //// GET: Checkout/Index
        //public IActionResult Index(int orderId)
        //{
        //    // Fetch the order using the orderId (or any other way you're managing your orders)
        //    var order = _context.Orders
        //                // .AsNoTracking() // Optimize query for read-only operation
        //                .Include(o => o.Customer) // Include related customer data
        //                .Include(o => o.OrderItems) // Include related order items
        //                .ThenInclude(oi => oi.Product) // Include product details in order items
        //                .FirstOrDefaultAsync(o => o.OrderId == orderId);

        //    //    if (order == null)
        //    //    {
        //    //        return NotFound(); // Return 404 if order not found
        //    //    }var order = GetOrderById(orderId);  // Replace with your logic to get the order
        //    if (order == null)
        //    {
        //        // Handle error: order not found
        //        return NotFound();
        //    }
        //    return View(order);
        //}

        ////[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> Checkout(int id) // id is the OrderId
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        //    {
        //        return RedirectToAction("LoginPage", "Login");
        //    }

        //    // Fetch the order, customer, and items based on the OrderId
        //    var order = await _context.Orders
        //               // .AsNoTracking() // Optimize query for read-only operation
        //                .Include(o => o.Customer) // Include related customer data
        //                .Include(o => o.OrderItems) // Include related order items
        //                .ThenInclude(oi => oi.Product) // Include product details in order items
        //                .FirstOrDefaultAsync(o => o.OrderId == id);

        //    if (order == null)
        //    {
        //        return NotFound(); // Return 404 if order not found
        //    }

        //    // Logging for debugging
        //    Console.WriteLine($"Order ID: {order.OrderId}, Total Items: {order.OrderItems?.Count ?? 0}");

        //    // Prepare the CheckoutViewModel
        //    var checkoutViewModel = new CheckoutViewModel
        //    {
        //        CustomerName = order.Customer?.CustomerName ?? "Guest", // Get from included Customer object
        //        OrderId = order.OrderId,
        //        TotalAmount = order.TotalPrice,
        //        PaymentMethod = order.PaymentMethod,
        //        OrderItems = (order.OrderItems ?? new List<OrderItem>()) // Ensure OrderItems is not null
        //            .Select(oi => new CheckoutItemViewModel
        //            {
        //                ProductName = oi.Product?.Name ?? "Unknown Product",
        //                Price = oi.Product?.Price ?? 0,
        //                Quantity = oi.Quantity,
        //                Subtotal = oi.Subtotal
        //            }).ToList()
        //    };

        //    // Pass the model to the view
        //    return View(checkoutViewModel);
        //}


        // [Authorize]
        [HttpPost]
        public IActionResult CheckoutOrder(CheckoutOrderModel model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            // Fetch the order from the Orders table
            var order = _context.Orders
                .Include(o => o.OrderItems)  // Include OrderItems for product details
                .FirstOrDefault(o => o.OrderId == model.OrderId);

            if (order == null)
            {
                // Return a NotFound response if the order does not exist
                return NotFound();
            }

            // Update the OrderStatus to "Order Confirmed"
            order.OrderStatus = "Order Confirmed";

            // Get the current user from the session
            var cashierId = HttpContext.Session.GetString("UserId") ?? "0"; // Default to "0" if null
            var cashierFullName = HttpContext.Session.GetString("UserFullName");

            // Parse the cashierId to int
            int parsedCashierId;
            if (!int.TryParse(cashierId, out parsedCashierId))
            {
                // Handle the error, e.g., return a BadRequest response
                return BadRequest("Invalid UserId in the session.");
            }

            // Calculate the change amount
            decimal? change = model.PaidAmount - order.TotalPrice;

            // Create a new Transaction
            var transaction = new Transaction
            {
                OrderId = model.OrderId,
                CashierId = parsedCashierId,  // Assuming UserId is stored as a string and needs to be parsed to int
                TotalAmount = order.TotalPrice,
                PaidAmount = model.PaidAmount,
                Change = change,
                PaymentStatus = "Paid",
                PaymentMethod = order.PaymentMethod,
                TransactionDate = DateTime.Now
            };

            // Add the transaction to the Transactions table
            _context.Transactions.Add(transaction);
            _context.SaveChanges(); // Save to get the generated TransactionId

            // Create TransactionItems for the order's OrderItems
            var stockUpdates = new List<ProductStockUpdateModel>();
            foreach (var orderItem in order.OrderItems)
            {
                var transactionItem = new TransactionItem
                {
                    TransactionId = transaction.TransactionId,  // Set the generated TransactionId
                    ProductId = orderItem.ProductId,  // ProductId from OrderItems
                    Quantity = orderItem.Quantity,  // Quantity from OrderItems
                    Subtotal = orderItem.Subtotal  // Subtotal from OrderItems
                };
                _context.TransactionItems.Add(transactionItem);

                // Prepare stock update data
                stockUpdates.Add(new ProductStockUpdateModel
                {
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity
                });
            }

            // Create a Payment record for the transaction
            var payment = new Payment
            {
                TransactionId = transaction.TransactionId,  // Set the generated TransactionId
                PaymentType = order.PaymentMethod,  // PaymentMethod from the Orders table
                Amount = model.PaidAmount,
                PaymentDate = DateTime.Now
            };
            _context.Payments.Add(payment);

            // Save all changes to the database
            _context.SaveChanges();

            // Call Inventory Service to update stock
            try
            {
                var inventoryService = new InventoryService(new HttpClient());
                inventoryService.UpdateProductStockAsync(stockUpdates).Wait();
                //await _context.SaveChangesAsync();
                //await inventoryService.UpdateProductStockAsync(stockUpdates);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update product stock: {ex.Message}");
            }

            TempData["ShowPopup"] = true; // Indicate that the popup should be shown
            TempData["PopupMessage"] = "Order has been successfully confirmed!";

            // Redirect to a success or confirmation page
            return RedirectToAction("Sales", "Sales");  // Replace with your actual success page or action
        }




        // [Authorize]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

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
        [Authorize]
        public IActionResult Confirmation()
        {
            return View();
        }

        //[HttpPost]4        [HttpPost]
        //[HttpGet]

        //public IActionResult CheckoutCopy(List<CartItem> cartItems, string customerName, string paymentMethod, decimal paymentAmount)
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

        //    //// Generate invoice
        //    //var invoice = new Invoice
        //    //{
        //    //    CustomerName = customerName,
        //    //    PaymentMethod = paymentMethod,
        //    //    PaymentAmount = paymentAmount,
        //    //    TotalAmount = totalAmount,
        //    //    ChangeAmount = changeAmount,
        //    //    DateCreated = DateTime.Now
        //    //};

        //    //// Save to database
        //    //_context.Invoices.Add(invoice);
        //    //_context.SaveChanges();

        //    // Redirect to Invoice page or show confirmation
        //    //return RedirectToAction("InvoiceDetails", new { id = invoice.InvoiceNumber });
        //    return RedirectToAction("Sales", "Sales");
        //}

        [HttpGet]
        public async Task<IActionResult> CheckoutCopy(int id) // id is the OrderId
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            // Fetch the order, customer, and items based on the OrderId
            var order = await _context.Orders
                .Include(o => o.Customer) // Include related customer data
                .Include(o => o.OrderItems) // Include related order items
                .ThenInclude(oi => oi.Product) // Include product details in order items
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                //return RedirectToAction("BlankCheckout"); // Return a specific view for this case
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
    }

}
