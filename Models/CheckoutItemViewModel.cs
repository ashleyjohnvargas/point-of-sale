namespace POS1.Models
{
	public class CheckoutItemViewModel
	{
		public string? ProductName { get; set; } // Product name
		public decimal? Price { get; set; } // Price of the product
		public int? Quantity { get; set; } // Quantity of the product
		public decimal? Subtotal { get; set; } // Subtotal (Price * Quantity)
	}
}
