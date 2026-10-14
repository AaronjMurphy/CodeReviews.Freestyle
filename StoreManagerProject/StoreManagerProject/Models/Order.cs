namespace StoreManagerProject.Models
{
    internal class Order
    {
        public required int Id { get; set; }
        public required string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedAt { get; init; }
        public DateTime ProcessedAt { get; set; }
    }
}
