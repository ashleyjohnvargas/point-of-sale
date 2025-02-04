namespace POS1.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? Date { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? OrderStatus { get; set; }
    }
}
