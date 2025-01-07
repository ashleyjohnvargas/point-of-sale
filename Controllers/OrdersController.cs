// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourApp.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }

        // This is a mock list of orders; in a real application, it would be fetched from a database.
        private static List<Order> _orders = new List<Order>
        {
            new Order { OrderId = 1, CustomerName = "John Doe", OrderDate = DateTime.Now.AddDays(-2), TotalAmount = 150.00m, PaymentMethod = "Credit Card", PaymentStatus = "Paid", OrderStatus = "Completed", InvoiceId = 101 },
            new Order { OrderId = 2, CustomerName = "Jane Smith", OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 80.50m, PaymentMethod = "PayPal", PaymentStatus = "Unpaid", OrderStatus = "Pending", InvoiceId = 102 },
            new Order { OrderId = 3, CustomerName = "Alice Johnson", OrderDate = DateTime.Now.AddDays(-5), TotalAmount = 200.75m, PaymentMethod = "Bank Transfer", PaymentStatus = "Paid", OrderStatus = "Processing", InvoiceId = 103 }
        };

        // Display a list of orders
        public IActionResult Index()
        {
            var orders = _orders;
            return View(orders);
        }
       
          

        // View an individual invoice by InvoiceId (In a real application, you would fetch the invoice from a database)
        public IActionResult ViewInvoice(int invoiceId)
        {
            // Here, you'd query the database to get the invoice details
            var invoiceDetails = $"Invoice details for Invoice ID: {invoiceId}";
            return Content(invoiceDetails);
        }


    }
}
