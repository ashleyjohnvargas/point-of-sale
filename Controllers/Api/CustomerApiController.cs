using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;
using System.Linq;

namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Route: api/CustomerApi/SyncCustomer
        [HttpPost("SyncCustomer")]
        public IActionResult SyncCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Invalid customer data.");

            try
            {
                // Check if customer already exists 
                var existingCustomer = _context.Customers.FirstOrDefault(c =>
                    c.CustomerName == customer.CustomerName ||
                    c.Email == customer.Email ||
                    c.Address == customer.Address ||
                    c.PhoneNumber == customer.PhoneNumber);

                if (existingCustomer != null)
                {
                    // Update the existing customer record
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.PhoneNumber = customer.PhoneNumber;

                    _context.Customers.Update(existingCustomer);
                    _context.SaveChanges();

                    return Ok(new { Message = "Customer updated successfully!" });
                }
                else
                {
                    // Add new customer record
                    _context.Customers.Add(customer);
                    _context.SaveChanges();

                    return Ok(new { Message = "Customer created successfully!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while syncing the customer.", Details = ex.Message });
            }
        }
    }
}
