namespace RestaurantOrderManager.Models
{
    internal class MenuItem
    {
        public required int Id { get; set; }
        public required string Code { get; init; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
        public HashSet<string> DietaryLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
