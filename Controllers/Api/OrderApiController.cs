using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;

namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Route: api/OrderApi/CreateOrder
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Invalid order data.");

            order.OrderId = 0; // Ensure Id is not set ex
            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(new { Message = "Order created successfully!" });
        }
    }
}
