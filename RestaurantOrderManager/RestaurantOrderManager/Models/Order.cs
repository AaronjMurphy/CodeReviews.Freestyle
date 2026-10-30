namespace RestaurantOrderManager.Models
{
    internal class Order
    {
        public required int Id { get; set; }
        public required int TableNumber { get; set; }
        public required int Items { get; set; }
        public required DateTime OrderedAt { get; set; } 
        public DateTime ProcessedAt { get; set; }

    }
}
