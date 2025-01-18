namespace POS1.Models
{
    public class ProductStockUpdateModel
    {
        public int? ProductId { get; set; }
        public int? Quantity { get; set; } // Quantity to deduct
    }
}
