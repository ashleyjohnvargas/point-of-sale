using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
	public class CheckoutOrderModel
	{
		public int? OrderId { get; set; } // Hidden field for Order ID
		public decimal? PaidAmount { get; set; } // Payment amount entered by the user
		public decimal? Change { get; set; } // Read-only field for change
	}
}
