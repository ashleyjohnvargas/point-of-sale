using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
    public class TransactionDto
    {
        public int? OrderId { get; set; }  // Foreign key (nullable)
        //public int? CashierId { get; set; } // Set the CashierId in the TransactionApiController by inserting the session UserId
        public decimal? TotalAmount { get; set; }  // Final transaction amount
        public decimal? PaidAmount { get; set; }  // Amount paid by the customer
        public decimal? Change { get; set; }  // Change returned to the customer
        public string? PaymentStatus { get; set; }  // Status of payment
        public string? PaymentMethod { get; set; }  // Chosen payment method
        public DateTime? TransactionDate { get; set; }  // Transaction timestamp

    }
}
