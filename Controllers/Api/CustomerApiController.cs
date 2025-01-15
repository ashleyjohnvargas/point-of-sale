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
        public IActionResult SyncCustomer([FromBody] CustomerDto customer)
        {
            if (customer == null)
                return BadRequest("Invalid customer data.");

            try
            {
                // Check if customer already exists 
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.EcomId == customer.CustomerId);

                if (existingCustomer != null)
                {
                    // Update the existing customer record
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.EcomId = customer.CustomerId;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.PhoneNumber = customer.PhoneNumber;

                    _context.Customers.Update(existingCustomer);
                    _context.SaveChanges();

                    return Ok(new { Message = "Customer updated successfully!" });
                }
                else
                {
                    // Map the DTO to the domain model and add the new product
                    var newCustomer = new Customer
                    {
                        //Id = productDto.Id, // NOTE: Ensure the IDENTITY_INSERT issue is resolved here
                        CustomerName = customer.CustomerName,
                        EcomId = customer.CustomerId,
                        Email = customer.Email,
                        Address = customer.Address,
                        PhoneNumber = customer.PhoneNumber,
                    };
                    // Add new customer record
                    _context.Customers.Add(newCustomer);
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
