using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;


namespace POS1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Route: api/ProductsApi/SyncProducts
        [HttpPost("SyncProducts")]
        public IActionResult SyncProducts([FromBody] List<ProductDto> productDtos)
        {
            foreach (var productDto in productDtos)
            {
                // Check if the product already exists
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == productDto.Id);
                if (existingProduct != null)
                {
                    // Update the existing product
                    existingProduct.Name = productDto.Name;
                    existingProduct.Description = productDto.Description;
                    existingProduct.Price = productDto.Price;
                    existingProduct.Color = productDto.Color;
                    existingProduct.Category = productDto.Category;
                    existingProduct.OriginalStock = productDto.OriginalStock;
                    existingProduct.CurrentStock = productDto.CurrentStock;
                    existingProduct.StockStatus = productDto.StockStatus;
                    existingProduct.IsBeingSold = productDto.IsBeingSold;
                    existingProduct.IsDeleted = productDto.IsDeleted;
                    existingProduct.DateAdded = productDto.DateAdded;
                }
                else
                {
                    // Map the DTO to the domain model and add the new product
                    var newProduct = new Product
                    {
                        //Id = productDto.Id, // NOTE: Ensure the IDENTITY_INSERT issue is resolved here
                        Name = productDto.Name,
                        Description = productDto.Description,
                        Price = productDto.Price,
                        Color = productDto.Color,
                        Category = productDto.Category,
                        OriginalStock = productDto.OriginalStock,
                        CurrentStock = productDto.CurrentStock,
                        StockStatus = productDto.StockStatus,
                        IsBeingSold = productDto.IsBeingSold,
                        IsDeleted = productDto.IsDeleted,
                        DateAdded = productDto.DateAdded
                    };
                    _context.Products.Add(newProduct);
                }
            }

            _context.SaveChanges();
            return Ok();
        }

    }
}
