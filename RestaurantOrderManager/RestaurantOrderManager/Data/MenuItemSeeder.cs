using RestaurantOrderManager.Models;

namespace RestaurantOrderManager.Data
{
    internal class MenuItemSeeder
    {
        internal static List<MenuItem> SeedMenuItems()
        {
            List<MenuItem> seedData = new List<MenuItem>(){

                new MenuItem
                {
                    Id = 1,
                    Code = "S001",
                    Name = "Sandwich",
                    Category = "Starters",
                    Price = 10.99m,
                    DietaryLabels = { "wheat", "dairy" }
                },

                new MenuItem
                {
                    Id = 2,
                    Code = "S002",
                    Name = "Soup",
                    Category = "Starters",
                    Price = 5.99m,
                    DietaryLabels = { "wheat" }

                },

                new MenuItem
                {
                    Id = 3,
                    Code = "M001",
                    Name = "Cheese Burger With Fries",
                    Category = "Mains",
                    Price = 14.99m,
                    DietaryLabels = { "wheat", "dairy" }
                },

                new MenuItem
                {
                    Id = 4,
                    Code = "M002",
                    Name = "Pasta",
                    Category = "Mains",
                    Price = 10.99m,
                    DietaryLabels = { "wheat", "eggs" }
                },

                new MenuItem
                {
                    Id = 5,
                    Code = "D001",
                    Name = "Ice-Cream",
                    Category = "Deserts",
                    Price = 5.50m,
                    DietaryLabels = { "dairy" }
                },

                new MenuItem
                {
                    Id = 6,
                    Code = "D002",
                    Name = "Chocolate Fudge Cake",
                    Category = "Deserts",
                    Price = 6.50m,
                    DietaryLabels = { "dairy", "eggs" }
                },

                new MenuItem
                {
                    Id = 7,
                    Code = "B001",
                    Name = "Wine",
                    Category = "Beverages",
                    Price = 4.50m,
                    DietaryLabels = { "alcohol" }
                },

                new MenuItem
                {
                    Id = 8,
                    Code = "B002",
                    Name = "Beer",
                    Category = "Beverages",
                    Price = 4.00m,
                    DietaryLabels = { "wheat", "alcohol"}
                }
            };

            return seedData;
        }
    }
}
