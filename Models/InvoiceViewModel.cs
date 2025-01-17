namespace POS1.Models
{
    public class InvoiceViewModel
    {
        public int? TransactionId { get; set; }
        public string? CustName { get; set; }
        public string? CashierName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public List<string>? Items { get; set; }
        public List<decimal?> Subtotals { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Change { get; set; }
        public int ItemCount { get; set; }
    }
}
