using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace POS1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

       // [Authorize]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("LoginPage", "Login");
            }

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            // A) Sales calculations
            var salesToday = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid" && t.TransactionDate.HasValue && t.TransactionDate.Value.Date == today)
                .Sum(t => t.TotalAmount);

            var salesThisWeek = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid" && t.TransactionDate.HasValue && t.TransactionDate.Value >= startOfWeek)
                .Sum(t => t.TotalAmount) ?? 0;

            var salesThisMonth = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid" && t.TransactionDate.HasValue && t.TransactionDate.Value >= startOfMonth)
                .Sum(t => t.TotalAmount) ?? 0;

            var salesThisYear = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid" && t.TransactionDate.HasValue && t.TransactionDate.Value >= startOfYear)
                .Sum(t => t.TotalAmount) ?? 0;

            // B) Orders per status
            var ordersStatusToday = _context.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today)
                .GroupBy(o => o.OrderStatus)
                .Select(g => new OrderStatusCount { Status = g.Key, Count = g.Count() })
                .ToList();

            var ordersStatusMonth = _context.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value >= startOfMonth)
                .GroupBy(o => o.OrderStatus)
                .Select(g => new OrderStatusCount { Status = g.Key, Count = g.Count() })
                .ToList();

            // C) Top selling products and categories
            var topSellingProducts = _context.TransactionItems
                .Where(ti => ti.Transaction != null && ti.Transaction.PaymentStatus == "Paid")
                .GroupBy(ti => ti.Product.Name)
                .OrderByDescending(g => g.Sum(ti => ti.Quantity))
                .Take(5)
                .Select(g => new TopSellingProduct { Product = g.Key, Quantity = (int)g.Sum(ti => ti.Quantity) })
                .ToList();

            var topCategories = _context.TransactionItems
                .Where(ti => ti.Transaction != null && ti.Transaction.PaymentStatus == "Paid")
                .GroupBy(ti => ti.Product.Category)
                .OrderByDescending(g => g.Sum(ti => ti.Quantity))
                .Select(g => new TopCategory { Category = g.Key, QuantitySold = (int)g.Sum(ti => ti.Quantity) })
                .Take(8)
                .ToList();

            // D) Payment methods
            var paymentMethods = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid")
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new PaymentMethodCount { Method = g.Key, Count = g.Count() })
                .ToList();

            // Prepare the result for the dashboard
            var result = new DashboardViewModel
            {
                SalesToday = (decimal)salesToday,
                SalesThisWeek = salesThisWeek,
                SalesThisMonth = salesThisMonth,
                SalesThisYear = salesThisYear,
                OrdersStatusToday = ordersStatusToday,
                OrdersStatusMonth = ordersStatusMonth,
                TopSellingProducts = topSellingProducts,
                TopCategories = topCategories,
                PaymentMethods = paymentMethods
            };

            if (ordersStatusToday.Count == 0)
            {
                Console.WriteLine("No Orders Today!");
            }


            return View(result);
        }





       // [Authorize]
        [HttpGet]
        public IActionResult GetDashboardData()
        {
            // Precompute date ranges outside of the LINQ query to avoid translation issues
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Start of the current week (Sunday)
            var startOfMonth = new DateTime(today.Year, today.Month, 1); // Start of the current month
            var startOfYear = new DateTime(today.Year, 1, 1); // Start of the current year

            // A) Sales calculations
            var salesToday = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid"
                    && t.TransactionDate.HasValue
                    && t.TransactionDate.Value.Date == today)
                .Sum(t => t.TotalAmount);

            var salesThisWeek = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid"
                    && t.TransactionDate.HasValue
                    && t.TransactionDate.Value >= startOfWeek)
                .Sum(t => t.TotalAmount) ?? 0;

            var salesThisMonth = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid"
                    && t.TransactionDate.HasValue
                    && t.TransactionDate.Value >= startOfMonth)
                .Sum(t => t.TotalAmount) ?? 0;

            var salesThisYear = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid"
                    && t.TransactionDate.HasValue
                    && t.TransactionDate.Value >= startOfYear)
                .Sum(t => t.TotalAmount) ?? 0;

            // B) Orders per status
            var ordersStatusToday = _context.Orders
                .Where(o => o.CreatedAt.HasValue
                    && o.CreatedAt.Value == today)
                .GroupBy(o => o.OrderStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            var ordersStatusMonth = _context.Orders
                .Where(o => o.CreatedAt.HasValue
                    && o.CreatedAt.Value >= startOfMonth)
                .GroupBy(o => o.OrderStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // C) Top selling products and categories
            var topSellingProducts = _context.TransactionItems
                .Where(ti => ti.Transaction != null && ti.Transaction.PaymentStatus == "Paid")
                .GroupBy(ti => ti.Product.Name)
                .OrderByDescending(g => g.Sum(ti => ti.Quantity))
                .Take(5)
                .Select(g => new { Product = g.Key, Quantity = g.Sum(ti => ti.Quantity) })
                .ToList();

            var topCategories = _context.TransactionItems
                .Where(ti => ti.Transaction != null && ti.Transaction.PaymentStatus == "Paid")
                .GroupBy(ti => ti.Product.Category)
                .OrderByDescending(g => g.Sum(ti => ti.Quantity))
                .Select(g => new { Category = g.Key, QuantitySold = g.Sum(ti => ti.Quantity) })
                .Take(5)
                .ToList();

            // D) Payment methods
            var paymentMethods = _context.Transactions
                .Where(t => t.PaymentStatus == "Paid")
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new { Method = g.Key, Count = g.Count() })
                .ToList();

            // Prepare the result for the dashboard
            var result = new
            {
                salesToday,
                salesThisWeek,
                salesThisMonth,
                salesThisYear,
                ordersStatusToday = ordersStatusToday.Select(x => new { x.Status, x.Count }).ToList(),
                ordersStatusMonth = ordersStatusMonth.Select(x => new { x.Status, x.Count }).ToList(),
                topSellingProducts = topSellingProducts.Select(x => new { x.Product, x.Quantity }).ToList(),
                topCategories = topCategories.Select(x => new { x.Category, x.QuantitySold }).ToList(),
                paymentMethods = paymentMethods.Select(x => new { x.Method, x.Count }).ToList()
            };

            return Json(result); // Return JSON data for frontend
        }




        //[HttpGet("logout")]
        //public IActionResult Logout()
        //{
        //    // Clear session or authentication cookies
        //    HttpContext.SignOutAsync();
        //    return RedirectToAction("LoginPage", "Login");
        //}

        [HttpGet]
        public IActionResult Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Remove all session data
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserFullName");
            HttpContext.Session.Remove("UserEmail");
            // Prevent back navigation from showing cached content
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            return RedirectToAction("LoginPage", "Login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
