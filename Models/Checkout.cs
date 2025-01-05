// Models/CheckoutModel.cs
using System.Collections.Generic;

namespace POS1.Models
{
    public class CheckoutModel
    {
        public required List<CartItem> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItem
    {
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
