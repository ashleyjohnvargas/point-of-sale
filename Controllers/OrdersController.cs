﻿// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Orders()
        {
            // Fetch all orders with necessary details
            var orders = _context.Orders
                .Include(o => o.Customer)  // Load associated Customer
                .Include(o => o.Invoices)  // Include Invoices for payment status
                .Select(o => new
                {
                    Id = o.OrderId,
                    // Find the customer name by matching CustomerId in Orders with EcomId in Customers
                    CustName = _context.Customers
                        .Where(c => c.EcomId == o.CustomerId) // Match EcomId with CustomerId
                        .Select(c => c.CustomerName)
                        .FirstOrDefault() ?? "No Customer",  // Safeguard for null values
                    Date = o.CreatedAt,
                    TotalAmount = o.TotalPrice,
                    PayMethod = o.PaymentMethod,
                    // Handle null invoices explicitly in the projection
                    PaymentStatus = o.Invoices.Any() ? o.Invoices.FirstOrDefault().PaymentStatus : "Not Available",
                    OrdStatus = o.OrderStatus
                })
                .ToList();

            return View(orders);
        }


        // Display a list of orders
        public IActionResult Index()
        {
            return View();
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
