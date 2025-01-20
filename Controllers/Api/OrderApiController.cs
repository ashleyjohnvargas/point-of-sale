using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using POS1.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EcommerceService _ecommerceService;

        public OrderApiController(ApplicationDbContext context, EcommerceService ecommerceService)
        {
            _dbContext = context;
            _ecommerceService = ecommerceService;
        }

        // Route: api/OrderApi/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }   

            var origOrderId = order.OrderId;
                
            order.OrderId = 0; // Ensure Id is not set ex
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // If the OrderStatus of the order is "Order Confirmed", create an instance of Transactions table
            if (order.OrderStatus == "Order Confirmed")
            {
                // Get the current user's ID from session
                int cashierId = HttpContext.Session.GetInt32("UserId") ?? 0;
                //if (cashierId == 0)
                //{
                //    return BadRequest("User session is invalid or UserId is not set.");
                //}

                var transaction = new Transaction()
                {
                    OrderId = origOrderId,
                    CashierId = cashierId,
                    TotalAmount = order.TotalPrice,
                    PaidAmount = order.TotalPrice,
                    Change = 0,
                    PaymentStatus = "Paid",
                    PaymentMethod = order.PaymentMethod,
                    TransactionDate = order.CreatedAt
                };
                _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();


                // Create a payment record
                var payment = new Payment()
                {
                    TransactionId = transaction.TransactionId,
                    PaymentType = order.PaymentMethod,
                    Amount = order.TotalPrice,
                    PaymentDate = order.CreatedAt // Use current date and time
                };
                _dbContext.Payments.Add(payment);
                _dbContext.SaveChanges();


                // Fetch the order items from the Ecommerce system
                try
                {
                    var orderItems = await _ecommerceService.GetOrderItems(origOrderId);

                    // Map order items to TransactionItems and save them
                    foreach (var item in orderItems)
                    {
                        var transactionItem = new TransactionItem
                        {
                            TransactionId = transaction.TransactionId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Subtotal = item.Subtotal
                        };

                        _dbContext.TransactionItems.Add(transactionItem);
                    }

                    await _dbContext.SaveChangesAsync();

                    // Prepare stock updates
                    var stockUpdates = orderItems.Select(item => new ProductStockUpdateModel
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }).ToList();

                    // Call Inventory Service to update stock
                    try
                    {
                        var inventoryService = new InventoryService(new HttpClient());
                        await inventoryService.UpdateProductStockAsync(stockUpdates);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Failed to update product stock: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error fetching order items: {ex.Message}");
                }

            }
            return Ok(new { Message = "Order created successfully!" });
        }
    }
}
