namespace POS1.Models
{
    public class SalesViewModel
    {
        public int TransactionId { get; set; }
        public string? CustomerName { get; set; }
        public decimal? Amount { get; set; }
        public int? OrderNumber { get; set; }
        public string? CashierName { get; set; }
        public DateTime? SaleDate { get; set; }
    }
}
