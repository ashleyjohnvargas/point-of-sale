// Controllers/CheckoutController.cs

namespace POS1.Controllers
{
    internal class Invoice
    {
        public string CustomerName { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}