namespace StoreManagerProject.Models
{
    public class Product
    {
        public required string Sku { get; init; }
        public required string Name { get; set; }
        public required string Department { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public HashSet<string> Tags { get; set; } = 
            new(StringComparer.OrdinalIgnoreCase);
    }
}
