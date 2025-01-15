namespace POS1.Models
{
    public class OrderItemCopy
    {
        public int? OrderId { get; set; }  // Reference to the associated order (nullable)
        public int? ProductId { get; set; }  // Reference to the product (nullable)
        public int? Quantity { get; set; }  // Quantity of the product in the order (nullable)
        public decimal? Subtotal { get; set; }  // Subtotal price for the product (nullable)
    }
}
