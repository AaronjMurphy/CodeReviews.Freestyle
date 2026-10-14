using StoreManagerProject.Models;
namespace StoreManagerProject.Data
{
    internal class OrderQueueSeeder
    {
        public static Queue<Order> GetOrderdata()
        {
            Order[] orders =
            {
                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Mary",
                    ItemCount = 10,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Jeff",
                    ItemCount = 25,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Jonathan",
                    ItemCount = 4,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Gareth",
                    ItemCount = 2,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Jennifer",
                    ItemCount = 13,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Martha",
                    ItemCount = 3,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Chris",
                    ItemCount = 55,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Jenz",
                    ItemCount = 25,
                    CreatedAt = DateTime.Now
                },

                new Order
                {
                    Id = StoreApp._nextOrderNumber++,
                    CustomerName = "Michi",
                    ItemCount = 67,
                    CreatedAt = DateTime.Now
                }

            };

            return new Queue<Order>(orders);
        }
    }
}
