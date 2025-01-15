using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;

namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoiceApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Route: api/InvoiceApi/CreateInvoice
        [HttpPost("CreateInvoice")]
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            if (invoice == null)
                return BadRequest("Invalid invoice data.");

            invoice.InvoiceId = 0;

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return Ok(new { Message = "Invoice created successfully!" });
        }
    }
}
