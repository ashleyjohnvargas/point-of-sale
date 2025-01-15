namespace POS1.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public string? Category { get; set; }
        public int OriginalStock { get; set; }
        public int CurrentStock { get; set; }
        public string? StockStatus { get; set; }
        public bool IsBeingSold { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
