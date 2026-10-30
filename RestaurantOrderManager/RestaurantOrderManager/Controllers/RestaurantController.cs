using RestaurantOrderManager.UI;
using RestaurantOrderManager.Enums;
using RestaurantOrderManager.Models;

namespace RestaurantOrderManager.Controllers
{
    internal class RestaurantController
    {
        internal static string[] MenuCategories = { "Starters", "Mains", "Deserts", "Beverages" };
       
        internal void Run()
        {
            bool exitApp = false;

            while (!exitApp)
            {
                UserInput.ClearDisplay();
                UserInput.PrintFiglet();
                string menuSelection = UserInput.GetMenuInput(Menus.Main);

                switch (menuSelection)
                {
                    case "View Menu Categories":
                        ViewMenuCategories();
                        break;
                    case "Manage Menu Items":
                        ManageMenuItems();
                        break;
                    case "Manage Orders":
                        ManageKitchenOrders();
                        break;
                    case "View Summary":
                        ViewRestaurantSummary(MenuCategories, ManageMenuItemsController.MenuItems, ManageMenuItemsController.RemovedMenuItems,
                            ManageKitchenOrdersController.OutstandingOrders, ManageKitchenOrdersController.ProcessedOrders);
                        break;
                    case "Exit":
                        exitApp = true;
                        break;
                }
            }
        }

        internal static void ViewMenuCategories()
        {
            UserInput.ClearDisplay();

            if (MenuCategories.Length == 0)
            {
                UserInput.Error("There are no categories to view.");
                UserInput.Continue();
                return;
            }

            TablePrinter.PrintCategoriesTable(MenuCategories);
            UserInput.Continue();
        }

        internal static void ManageMenuItems()
        {
            bool returnToMainMenu = false;

            while (!returnToMainMenu)
            {
                UserInput.ClearDisplay();
                string menuSelection = UserInput.GetMenuInput(Menus.ManageMenuItems);

                switch (menuSelection)
                {
                    case "View all Menu Items":
                        ManageMenuItemsController.ViewAllMenuItems();
                        break;
                    case "Add Menu Item":
                        ManageMenuItemsController.AddMenuItem();
                        break;
                    case "Delete Menu Item":
                        ManageMenuItemsController.DeleteMenuItem();
                        break;
                    case "Update Menu Item":
                        ManageMenuItemsController.UpdateMenuItem();
                        break;
                    case "Undo Removal":
                        ManageMenuItemsController.UndoRemoval();
                        break;
                    case "Find Menu Item":
                        ManageMenuItemsController.FindMenuItem();
                        break;
                    case "Manage Dietary Labels":
                        ManageMenuItemsController.ManageDietaryLabels();
                        break;
                    case "Exit to Previous Menu":
                        returnToMainMenu = true;
                        break;
                }
            }
        }

        internal static void ManageKitchenOrders()
        {
            UserInput.ClearDisplay();
            bool returnToMainMenu = false;

            while (!returnToMainMenu)
            {
                UserInput.ClearDisplay();
                string menuSelection = UserInput.GetMenuInput(Menus.ManageKitchenOrders);

                switch (menuSelection)
                {
                    case "View all Outstanding Orders":
                        ManageKitchenOrdersController.ViewAllOutstandingOrders();
                        break;
                    case "Add Order":
                        ManageKitchenOrdersController.AddOrder();
                        break;
                    case "Process Order":
                        ManageKitchenOrdersController.ProcessOrder();
                        break;
                    case "View Order History":
                        ManageKitchenOrdersController.ViewOrderHistory();
                        break;
                    case "Exit to Previous Menu":
                        returnToMainMenu = true;
                        break;
                }
            }
        }

        internal static void ViewRestaurantSummary(string[] categories, List<MenuItem> menuItems, Stack<MenuItem> recoverableMenuItems, Queue<Order> outstandingOrders, List<Order> processedOrders)
        {
            UserInput.ClearDisplay();

            if (!categories.Any() && !menuItems.Any() && !recoverableMenuItems.Any() && !outstandingOrders.Any() && !processedOrders.Any())
            {
                UserInput.Error("There is no data to display.");
                UserInput.Continue();
                return;
            }
            TablePrinter.PrintRestaurantSummary(categories, menuItems, recoverableMenuItems, outstandingOrders, processedOrders);
            UserInput.Continue();
        }
        
    }
}
