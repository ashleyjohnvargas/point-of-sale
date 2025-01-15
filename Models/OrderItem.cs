using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS1.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }  // Unique identifier for each item in the order

        //[ForeignKey("Order")]
        public int? OrderId { get; set; }  // Reference to the associated order (nullable)

        //[ForeignKey("Product")]
        public int? ProductId { get; set; }  // Reference to the product (nullable)
        public int? Quantity { get; set; }  // Quantity of the product in the order (nullable)
        public decimal? Subtotal { get; set; }  // Subtotal price for the product (nullable)

        // Navigation property for the related Order and Product
        public virtual Order? Order { get; set; }  // Navigation property to the related Order
        public virtual Product? Product { get; set; }  // Navigation property to the related Product
    }
}
