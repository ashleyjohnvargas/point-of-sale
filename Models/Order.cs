using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS1.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }  // Unique identifier for the order

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }  // Reference to the customer placing the order (nullable)
        public decimal? TotalPrice { get; set; }  // Final price (subtotal + tax + shipping)
        public string? PaymentMethod { get; set; }  // Chosen payment method
        public string? OrderStatus { get; set; }  // Status of the order (e.g., Pending, Shipped, Delivered)
        public DateTime? CreatedAt { get; set; }  // Timestamp when the order was placed (nullable)

        // Navigation property for the related Customer (Customer)
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; } // Add this property

    }
}
