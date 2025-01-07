// Models/Order.cs
using System;

namespace POS1.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public required string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string PaymentMethod { get; set; }
        public required string PaymentStatus { get; set; }
        public required string OrderStatus { get; set; }
        public int InvoiceId { get; set; } // Assume each order has an associated invoice
    }

    public class StatusUpdateModel
    {
        public int InvoiceNumber { get; set; }
        public string NewValue { get; set; }
    }

}
