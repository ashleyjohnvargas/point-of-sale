using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;

namespace POS1.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }
       // [Authorize]
        public IActionResult Sales()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            // Fetch all transactions, including customer and cashier details
            var sales = _context.Transactions
                .Include(t => t.Order) // Include Order for OrderId
                .Include(t => t.Cashier) // Include Cashier (assuming a Users table)
                .Include(t => t.Order.Customer) // Include Customer
                .Select(t => new SalesViewModel
                {
                    TransactionId = t.TransactionId,
                    //CustomerName = t.Order.Customer.CustomerName,
                    CustomerName = _context.Customers
                            .Where(c => c.EcomId == t.Order.CustomerId) // Match EcomId with Order.CustomerId
                            .Select(c => c.CustomerName)
                            .FirstOrDefault() ?? "Guest",
                    Amount = t.TotalAmount,
                    OrderNumber = t.OrderId,
                    CashierName = t.Cashier.FullName,
                    SaleDate = t.TransactionDate,
                    OrderStatus = t.Order.OrderStatus
                })
                .ToList();

            return View(sales);
        }

      //  [Authorize]
        public IActionResult Invoice(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            // Fetch the transaction based on the TransactionId
            var transaction = _context.Transactions
                .Include(t => t.TransactionItems!) // Assert non-nullability
                .ThenInclude(ti => ti.Product)
                .FirstOrDefault(t => t.TransactionId == id);


            if (transaction == null)
            {
                return NotFound();
            }

            // Retrieve cashier name from the session
            var cashierName = HttpContext.Session.GetString("UserFullName");

            // Prepare the view model for the Invoice view
            var invoiceViewModel = new InvoiceViewModel
            {
                TransactionId = transaction.TransactionId,
                CustName = _context.Orders
                    .Where(o => o.OrderId == transaction.OrderId) // Match OrderId in Transactions and Orders
                    .Join(_context.Customers, // Join with Customers table
                          o => o.CustomerId, // Use CustomerId from Orders
                          c => c.EcomId,     // Match it with EcomId in Customers
                          (o, c) => c.CustomerName) // Select CustomerName
                    .FirstOrDefault() ?? "Guest", // Safeguard for null values
                CashierName = cashierName,
                TransactionDate = transaction.TransactionDate,
                Items = transaction.TransactionItems?.Select(ti => ti.Product.Name).ToList() ?? new List<string>(),
                Subtotals = transaction.TransactionItems?.Select(ti => ti.Subtotal).ToList() ?? new List<decimal?>(),
                TotalAmount = transaction.TotalAmount,
                Cash = transaction.PaidAmount,
                Change = transaction.Change,
                ItemCount = transaction.TransactionItems?.Count ?? 0
            };

            // Return the view with the view model
            return View(invoiceViewModel);
        }

    }
}
