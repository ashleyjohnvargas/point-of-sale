namespace POS1.Models
{
	public class CheckoutViewModel
	{
		public string? CustomerName { get; set; } // Name of the customer
		public int? OrderId { get; set; } // Order ID
		public decimal? TotalAmount { get; set; } // Total order amount
		public string? PaymentMethod { get; set; } // Payment method
		public List<CheckoutItemViewModel>? OrderItems { get; set; } // Items in the order
	}
}
