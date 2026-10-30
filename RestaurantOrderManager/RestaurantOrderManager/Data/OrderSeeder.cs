using RestaurantOrderManager.Models;

namespace RestaurantOrderManager.Data
{
    internal class OrderSeeder
    {
        internal static List<Order> SeedOrders()
        {
            List<Order> seedData = new List<Order>()
            {
                new Order
                {
                    Id = 1,
                    TableNumber = 5,
                    Items = 3,
                    OrderedAt = DateTime.Now
                },

                new Order
                {
                    Id = 2,
                    TableNumber = 7,
                    Items = 1,
                    OrderedAt = DateTime.Now
                },

                new Order
                {
                    Id = 3,
                    TableNumber = 2,
                    Items = 4,
                    OrderedAt = DateTime.Now
                },

                new Order
                {
                    Id = 4,
                    TableNumber = 9,
                    Items = 2,
                    OrderedAt = DateTime.Now
                },

                new Order
                {
                    Id = 5,
                    TableNumber = 3,
                    Items = 3,
                    OrderedAt = DateTime.Now
                },

                new Order
                {
                    Id = 6,
                    TableNumber = 1,
                    Items = 2,
                    OrderedAt = DateTime.Now
                }
            };

            return seedData;
        }
    }
}
