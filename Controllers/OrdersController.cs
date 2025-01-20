// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using POS1.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EcommerceService _ecommerceService;
        public OrdersController(ApplicationDbContext context, EcommerceService ecommerceService)
        {
            _context = context;
            _ecommerceService = ecommerceService;
        }

        //    public IActionResult Orders()
        //    {
        //        // Fetch all orders with necessary details  
        //        var orders = _context.Orders
        //.Where(o => !o.IsDeleted && o.OrderStatus == "Order Placed") // Exclude soft-deleted orders and filter by OrderStatus
        //.Include(o => o.Customer)  // Load associated Customer
        //            .Include(o => o.Invoices)  // Include Invoices for payment status
        //            .Select(o => new
        //            {
        //                Id = o.OrderId,
        //                // Find the customer name by matching CustomerId in Orders with EcomId in Customers
        //                CustName = _context.Customers
        //                    .Where(c => c.EcomId == o.CustomerId) // Match EcomId with CustomerId
        //                    .Select(c => c.CustomerName)
        //                    .FirstOrDefault() ?? "No Customer",  // Safeguard for null values
        //                Date = o.CreatedAt,
        //                TotalAmount = o.TotalPrice,
        //                PayMethod = o.PaymentMethod,
        //                // Handle null invoices explicitly in the projection
        //                PaymentStatus = o.Invoices.Any() ? o.Invoices.FirstOrDefault().PaymentStatus : "Not Available",
        //                OrdStatus = o.OrderStatus
        //            })
        //            .ToList();

        //        return View(orders);
        //    }

        public IActionResult Orders()
        {
            // Get the current date
            var currentDate = DateTime.Now.Date;

            // Fetch all orders with necessary details
            var orders = _context.Orders
                .Where(o =>
                    !o.IsDeleted && // Exclude soft-deleted orders
                    (o.OrderStatus == "Order Placed" ||
                    (o.OrderStatus == "Refunded" && o.CreatedAt > currentDate)) // Include "Refunded" orders with CreatedAt after today
                )
                .Include(o => o.Customer)  // Load associated Customer
                .Include(o => o.Invoices)  // Include Invoices for payment status
                .OrderBy(o => o.CreatedAt)  // Order by CreatedAt in ascending order (latest orders at the bottom)
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



        [HttpPost]
        public IActionResult ProcessRefund(RefundRequest request)
        {
            //// Validate input
            //if (string.IsNullOrEmpty(request.OrderNumber) || string.IsNullOrEmpty(request.TransactionId))
            //{
            //    TempData["PopupMessage"] = "Invalid refund request.";
            //    TempData["ShowPopup"] = true;
            //    return RedirectToAction("Index");
            //}

            // Find the order using the OrderNumber
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == request.OrderNumber);
            if (order == null)
            {
                TempData["PopupMessage"] = "Order not found.";
                TempData["ShowPopup"] = true;
                return RedirectToAction("Index");
            }

            // Update the order status and created date
            order.OrderStatus = "Refunded";
            order.CreatedAt = request.RefundDate;

            // Find the transaction using the TransactionId
            var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == request.TransactionId);
            if (transaction == null)
            {
                TempData["PopupMessage"] = "Transaction not found.";
                TempData["ShowPopup"] = true;
                return RedirectToAction("Index");
            }

            // Update the payment status
            transaction.PaymentStatus = "Refunded";

            var invoice = _context.Invoices.FirstOrDefault(t => t.OrderId == request.OrderNumber);
            invoice.PaymentStatus = "Refunded";

            // Save changes to the database
            _context.SaveChanges();

            // Prepare the model for the service request
            var refundModel = new OrderRefundModel
            {
                OrderId = order.OrderId,
                RefundDate = order.CreatedAt
            };

            // Call the service to update the Ecommerce system (this is just an example)
            //var ecommerceService = new EcommerceService(); // Assume this is a service to call the API
            _ecommerceService.UpdateOrderInEcommerce(refundModel);


            TempData["ShowPopup"] = true;
            TempData["PopupMessage"] = "Order refunded successfully.";

            return RedirectToAction("Sales", "Sales");
        }



        [HttpPost]
		public IActionResult DeleteOrder(int id)
		{
			// Fetch the order by ID
			var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);

			if (order == null)
			{
				// Handle the case where the order does not exist
				return NotFound();
			}

			// Set the OrderStatus to "Cancelled"
			order.OrderStatus = "Cancelled";

			// Soft delete the order by setting IsDeleted to true
			order.IsDeleted = true;

			// Save changes to the database
			_context.SaveChanges();

			// Redirect back to the Orders view
			return RedirectToAction("Orders");
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
