using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS1.Models;
using System.Linq;

namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Route: api/OrderItemApi/CreateOrderItems
        [HttpPost("CreateOrderItems")]
        public async Task<IActionResult> CreateOrderItems([FromBody] List<OrderItemCopy> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
            {
                return BadRequest("Order items cannot be null or empty.");
            }

            // Process the order items (e.g., save to the database)
            foreach (var item in orderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Order items created successfully.");
        }

    }
}
