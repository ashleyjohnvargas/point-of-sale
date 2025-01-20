namespace POS1.Models
{
    public class RefundRequest
    {
        public int OrderNumber { get; set; }
        public DateTime RefundDate { get; set; }
        public int TransactionId { get; set; }
    }
}
