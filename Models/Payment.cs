using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
	public class Payment
	{
		[Key]
		public int PaymentId { get; set; }  // Primary key

		[ForeignKey("Transaction")]
		public int? TransactionId { get; set; }  // Foreign key to Transactions table
		public string? PaymentType { get; set; }  // Type of payment (e.g., Credit Card, Cash)
		public decimal? Amount { get; set; }  // Amount paid
		public DateTime? PaymentDate { get; set; }  // Payment timestamp

		public virtual Transaction? Transaction { get; set; }
	}
}
