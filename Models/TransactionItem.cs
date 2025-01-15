using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
	public class TransactionItem
	{
		[Key]
		public int TransactionItemId { get; set; }  // Primary key

		[ForeignKey("Transaction")]
		public int? TransactionId { get; set; }  // Foreign key to Transactions table

		[ForeignKey("Product")]
		public int? ProductId { get; set; }  // Foreign key to Products table
		public int? Quantity { get; set; }  // Quantity of the product
		public decimal? Subtotal { get; set; }  // Subtotal price for the product

		public virtual Transaction? Transaction { get; set; }
		public virtual Product? Product { get; set; }
	}
}
