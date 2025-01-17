using System.ComponentModel.DataAnnotations.Schema;

namespace POS1.Models
{
    public class PaymentDto
    {
        public int? TransactionId { get; set; }  // Foreign key to Transactions table
        public string? PaymentType { get; set; }  // Type of payment (e.g., Credit Card, Cash)
        public decimal? Amount { get; set; }  // Amount paid
        public DateTime? PaymentDate { get; set; }  // Payment timestamp
    }
}
