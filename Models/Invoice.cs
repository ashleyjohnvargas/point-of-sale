using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace POS1.Models
{
    public class Invoice //ano dapat ilagay here?
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }

        public string CustomerName { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
