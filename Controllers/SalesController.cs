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

        public IActionResult Sales()
        {
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
                    SaleDate = t.TransactionDate
                })
                .ToList();

            return View(sales);
        }
    }
}
