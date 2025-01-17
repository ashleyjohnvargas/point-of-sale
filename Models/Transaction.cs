using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
	public class Transaction
	{
		[Key]
		public int TransactionId { get; set; }  // Primary key

		//[ForeignKey("Order")]
		public int? OrderId { get; set; }  // Foreign key (nullable)
		//[ForeignKey("Cashier")]
		public int? CashierId { get; set; }
		public decimal? TotalAmount { get; set; }  // Final transaction amount
		public decimal? PaidAmount { get; set; }  // Amount paid by the customer
		public decimal? Change { get; set; }  // Change returned to the customer
		public string? PaymentStatus { get; set; }  // Status of payment
		public string? PaymentMethod { get; set; }  // Chosen payment method
		public DateTime? TransactionDate { get; set; }  // Transaction timestamp

		// Navigation property for the related Customer (Customer)
		public virtual Order? Order { get; set; }
		public virtual Users? Cashier { get; set; }
		public virtual ICollection<TransactionItem>? TransactionItems { get; set; } // Add this property
	}
}
