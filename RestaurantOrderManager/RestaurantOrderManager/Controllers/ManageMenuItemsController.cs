using RestaurantOrderManager.UI;
using RestaurantOrderManager.Enums;
using RestaurantOrderManager.Models;
using RestaurantOrderManager.Data;

namespace RestaurantOrderManager.Controllers
{
    internal class ManageMenuItemsController
    {
        internal static List<MenuItem> MenuItems = new List<MenuItem>(MenuItemSeeder.SeedMenuItems());
        internal static Dictionary<string, MenuItem> MenuItemsByUniqueCode = MenuItems.ToDictionary(menuItem => menuItem.Code, StringComparer.OrdinalIgnoreCase);
        internal static int NextMenuItemId = GetNextIdNumber();
        internal static Stack<MenuItem> RemovedMenuItems = new Stack<MenuItem>();

        internal static void ViewAllMenuItems()
        {
            if (!MenuItems.Any())
            {
                UserInput.Error("There are no menu items to display.");
                UserInput.Continue();
                return;
            }
            TablePrinter.PrintMenuItemsTable(MenuItems);
            UserInput.Continue();
        }

        internal static void AddMenuItem()
        {
            UserInput.ClearDisplay();

            MenuItem newMenuItem = UserInput.GetNewMenuItemDetails();
            if (ValidateMenuItem(newMenuItem))
            {
                UserInput.Error($"An item already exists with that name or code.");
                UserInput.Continue();
                return;
            }

            if (!UserInput.Confirm("Add this item to the menu?"))
            {
                UserInput.WriteMessage("Item was not added.");
                UserInput.Continue();
                return;
            }

            SyncMenuItemAddition(newMenuItem);
            UserInput.WriteMessage("Item successfully added to the menu.");
            UserInput.Continue();
        }

        internal static bool ValidateMenuItem(MenuItem newMenuItem)
        {
            bool nameExists = MenuItems.Any(item => item.Name.Equals(newMenuItem.Name, StringComparison.OrdinalIgnoreCase));

            bool codeExists = MenuItemsByUniqueCode.ContainsKey(newMenuItem.Code);

            return nameExists || codeExists;
        }

        internal static void SyncMenuItemAddition(MenuItem newMenuItem)
        {
            MenuItems.Add(newMenuItem);
            MenuItemsByUniqueCode.Add(newMenuItem.Code, newMenuItem);
            NextMenuItemId++;
        }

        internal static void DeleteMenuItem()
        {
            if (!MenuItemsByUniqueCode.Any())
            {
                UserInput.Error("There are no menu items to delete.");
                UserInput.Continue();
                return;
            }
            MenuItem itemToDelete = UserInput.GetMenuItemByCode();
            UserInput.ClearDisplay();
            if (!UserInput.Confirm($"Are you sure you want to delete item '{itemToDelete.Code}({itemToDelete.Name})'?"))
            {
                UserInput.WriteMessage("Item was not deleted.");
                UserInput.Continue();
                return;
            }
            SyncMenuItemDeletion(itemToDelete);
            UserInput.WriteMessage("Item deleted successfully.");
            UserInput.Continue();
        }

        private static void SyncMenuItemDeletion(MenuItem itemToDelete)
        {
            MenuItems.Remove(itemToDelete);
            MenuItemsByUniqueCode.Remove(itemToDelete.Code);
            RemovedMenuItems.Push(itemToDelete);
        }

        internal static void UpdateMenuItem()
        {
            if (!MenuItemsByUniqueCode.Any())
            {
                UserInput.Error("There are no menu items to update.");
                UserInput.Continue();
                return;
            }
            MenuItem itemToUpdate = UserInput.GetMenuItemByCode();
            (string name, string category, decimal price) updatedItemDetails = UserInput.GetUpdatedMenuItemDetails();

            if (!UserInput.Confirm($"Would you like to update menu item '{itemToUpdate.Code}({itemToUpdate.Name})'?"))
            {
                UserInput.WriteMessage("Menu item was not updated.");
                UserInput.Continue();
                return;
            }

            itemToUpdate.Name = updatedItemDetails.name;
            itemToUpdate.Category = updatedItemDetails.category;
            itemToUpdate.Price = updatedItemDetails.price;

            UserInput.WriteMessage("Item updated successfully.");
            UserInput.Continue();
        }

        internal static void UndoRemoval()
        {
            if (!RemovedMenuItems.Any())
            {
                UserInput.Error("There are no removed menu items.");
                UserInput.Continue();
                return;
            }
            while (RemovedMenuItems.TryPeek(out MenuItem? lastRemovedItem))
            {
                UserInput.ClearDisplay();

                if (UserInput.Confirm($"Would you like to undo the removal of item '{lastRemovedItem.Code}({lastRemovedItem.Name})'?"))
                {
                    RemovedMenuItems.Pop();
                    SyncMenuItemAddition(lastRemovedItem);
                    UserInput.WriteMessage("Item restored!");
                    UserInput.Continue();
                }
                else
                {
                    UserInput.WriteMessage("Restoration cancelled.");
                    UserInput.Continue();
                    return;
                }
            }
            UserInput.ClearDisplay();
            UserInput.WriteMessage("There are no more items to restore.");
            UserInput.Continue();
        }

        internal static void FindMenuItem()
        {
            if (!MenuItemsByUniqueCode.Any())
            {
                UserInput.Error("There are no menu items to find.");
                UserInput.Continue();
                return;
            }

            MenuItem foundItem = UserInput.GetMenuItemByCode();
            UserInput.WriteMessage("Item Details:\n");
            UserInput.FoundMenuItemDetails(foundItem);

            UserInput.Continue();
        }

        internal static void ManageDietaryLabels()
        {
            bool returnToPreviousMenu = false;

            MenuItem itemToManage = UserInput.GetMenuItemByCode();
            while (!returnToPreviousMenu)
            {
                UserInput.ClearDisplay();
                string menuSelection = UserInput.GetMenuInput(Menus.ManageDietaryLabels);

                switch (menuSelection)
                {
                    case "Add Dietary Label":
                        ManageDietaryLabelsController.AddLabel(itemToManage);
                        break;
                    case "Remove Dietary Label":
                        ManageDietaryLabelsController.RemoveLabel(itemToManage);
                        break;
                    case "Exit to Previous Menu":
                        returnToPreviousMenu = true;
                        break;
                }
            }
        }

        private static int GetNextIdNumber()
        {
            if (!MenuItems.Any())
            {
                return 0;
            }
            int nextNumber = MenuItems.Max(item => item.Id);

            return nextNumber+1;
        }
    }
}
