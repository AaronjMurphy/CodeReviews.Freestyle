using StoreManagerProject.Models;

namespace StoreManagerProject.Data
{
    public class ProductListSeeder
    {

        public static List<Product> GetProductData()
        {
            List<Product> Products = new()
            {
                new Product
                {
                    Sku = "ELE-001",
                    Name = "Keyboard",
                    Department = "Electronics",
                    Price = 100.00m,
                    StockQuantity = 20,
                    Tags = {"tech", "wireless", "accessory", "gaming"}
                },

                new Product
                {
                    Sku = "GRO-001",
                    Name = "Pasta",
                    Department = "Groceries",
                    Price = 4.80m,
                    StockQuantity = 100,
                    Tags = {"food", "italian", "pasta"}
                },

                new Product
                {
                    Sku = "CLO-001",
                    Name = "Jacket",
                    Department = "Clothing",
                    Price = 50.00m,
                    StockQuantity = 10,
                    Tags = {"leather", "medium", "casual"}
                },

                new Product
                {
                    Sku = "FUR-001",
                    Name = "Drawer",
                    Department = "Furniture",
                    Price = 140.00m,
                    StockQuantity = 8,
                    Tags = {"wood", "DIY", "storage"}
                },

                new Product
                {
                    Sku = "T&G-001",
                    Name = "Chess Set",
                    Department = "Toys & Gifts",
                    Price = 25.00m,
                    StockQuantity = 20,
                    Tags = {"logic", "games", "wood"}
                },

                new Product
                {
                    Sku = "ELE-002",
                    Name = "Mouse",
                    Department = "Electronics",
                    Price = 60.00m,
                    StockQuantity = 15,
                    Tags = {"tech", "wireless", "accessory", "gaming"}

                },

                new Product
                {
                    Sku = "GRO-002",
                    Name = "Tissue Box",
                    Department = "Groceries",
                    Price = 5.00m,
                    StockQuantity = 200,
                    Tags = {"cleaning", "colds", "soothing"}
                },

                new Product
                {
                    Sku = "CLO-002",
                    Name = "Running Shoes",
                    Department = "Clothing",
                    Price = 45.00m,
                    StockQuantity = 30,
                    Tags = {"canvas", "large", "skating"}
                },

                new Product
                {
                    Sku = "FUR-002",
                    Name = "Bookshelf",
                    Department = "Furniture",
                    Price = 165.00m,
                    StockQuantity = 5,
                    Tags = {"wood", "storage", "books"}
                },

                new Product
                {
                    Sku = "T&G-002",
                    Name = "Nintendo Switch",
                    Department = "Toys & Gifts",
                    Price = 400.00m,
                    StockQuantity = 17,
                    Tags = {"gaming", "nintendo", "consoles"}
                },
            };

            return Products;
        }
    }
}
