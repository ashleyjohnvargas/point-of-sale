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
