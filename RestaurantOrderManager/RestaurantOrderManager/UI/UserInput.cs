using RestaurantOrderManager.Enums;
using Spectre.Console;
using RestaurantOrderManager.Models;
using RestaurantOrderManager.Controllers;

namespace RestaurantOrderManager.UI
{
    internal class UserInput
    {
        private static string[] MainMenuOptions = { "View Menu Categories", "Manage Menu Items", "Manage Orders", "View Summary", "Exit" };
        private static string[] ManageMenuItemOptions = { "View all Menu Items", "Add Menu Item", "Delete Menu Item", "Update Menu Item", "Undo Removal", "Find Menu Item", "Manage Dietary Labels", "Exit to Previous Menu" };
        private static string[] ManageKitchenOrders = { "View all Outstanding Orders", "Add Order", "Process Order", "View Order History", "Exit to Previous Menu" };
        private static string[] ManageDietaryLabelsOptions = { "Add Dietary Label", "Remove Dietary Label", "Exit to Previous Menu" };
        private static int[] TableNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        internal static string GetMenuInput(Menus menuType)
        {
            string title = "";
            string[] menuOptions = {};

            switch (menuType)
            {
                case Menus.Main:
                    title = "Select an option from the menu:";
                    menuOptions = MainMenuOptions;
                    break;
                case Menus.ManageMenuItems:
                    title = "Select an option to manage menu items:";
                    menuOptions = ManageMenuItemOptions;
                    break;
                case Menus.ManageKitchenOrders:
                    title = "Select an option to manage kitchen orders:";
                    menuOptions = ManageKitchenOrders;
                    break;
                case Menus.ManageDietaryLabels:
                    title = "Select an option to manage items dietary labels";
                    menuOptions = ManageDietaryLabelsOptions;
                    break;
            }

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[white bold]{title}[/]")
                    .AddChoices(menuOptions));
        }

        internal static MenuItem GetNewMenuItemDetails()
        {
            WriteMessage("Please Enter the Details For the New Menu Item.\n");

            string code = AnsiConsole.Prompt(
                new TextPrompt<string>("[white bold]Unique Code (e.g. S001):[/] ")
                .Validate(input => !string.IsNullOrWhiteSpace(input)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Please enter a valid code.[/]")));

            string name = AnsiConsole.Prompt(
                new TextPrompt<string>("[white bold]Name:[/] ")
                .Validate(input => !string.IsNullOrWhiteSpace(input)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Please enter a valid name.[/]")));

            string category = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[white bold]Category:[/] ")
                .AddChoices(RestaurantController.MenuCategories));

            decimal price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[white bold]Price:[/] ")
                .Validate(input => input >= 0
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Price can not be negative.[/]")));

            return new MenuItem
            {
                Id = ManageMenuItemsController.NextMenuItemId,
                Code = code,
                Name = name,
                Category = category,
                Price = price
            };

        }

        internal static (string, string, decimal) GetUpdatedMenuItemDetails()
        {
            WriteMessage("Please Enter the Updated Details for the Menu Item.\n");

            string name = AnsiConsole.Prompt(
                new TextPrompt<string>("[white bold]Name:[/] ")
                .Validate(input => !string.IsNullOrWhiteSpace(input)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Please enter a valid name.[/]")));

            string category = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[white bold]Category:[/] ")
                .AddChoices(RestaurantController.MenuCategories));

            decimal price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[white bold]Price:[/] ")
                .Validate(input => input >= 0
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Price can not be negative.[/]")));

            return (name, category, price);
        }

        internal static MenuItem GetMenuItemByCode()
        {
            string code = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[white bold]Select the code of the menu item:[/] ")
                .AddChoices(ManageMenuItemsController.MenuItemsByUniqueCode.Keys));

            return ManageMenuItemsController.MenuItemsByUniqueCode[code];
        }

        internal static void FoundMenuItemDetails(MenuItem foundMenuItem)
        {
            AnsiConsole.MarkupLine($"[white bold]ID:[/] " +
                $"{foundMenuItem.Id}\n[white bold]Code:[/] {foundMenuItem.Code}\n[white bold]Name:[/] {foundMenuItem.Name}\n[white bold]Category:[/] " +
                $"{foundMenuItem.Category}\n[white bold]Price:[/] {foundMenuItem.Price}\n[white bold]Dietary Labels:[/] {string.Join(", ", foundMenuItem.DietaryLabels)}");
        }

        internal static string GetNewLabel(MenuItem itemToManage)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"[white bold]Enter a dietary label for '{itemToManage.Code}({itemToManage.Name})':[/] ")
                .Validate(label =>
                {
                    if (itemToManage.DietaryLabels.Contains(label))
                    {
                        return ValidationResult.Error("[red italic]This menu item already has that label.[/]");
                    }

                    else if (string.IsNullOrWhiteSpace(label))
                    {
                        return ValidationResult.Error("[red italic]Please enter a valid label.[/]");
                    }

                    else 
                        return ValidationResult.Success();
                }));
        }

        internal static string GetDietaryLabel(MenuItem itemToManage)
        {
            if (!itemToManage.DietaryLabels.Any())
            {
                Error("There are no labels to remove.");
                Continue();
                return string.Empty;
            }

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[white bold]Select the Label you Wish to Remove:[/]")
                .AddChoices(itemToManage.DietaryLabels));
        }

        internal static Order GetNewOrderDetails()
        {
            int tableNumber = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("[white bold]Table Number:[/] ")
                .AddChoices(TableNumbers));

            int itemCount = AnsiConsole.Prompt(
                new TextPrompt<int>("[white bold]Item Count:[/] ")
                .Validate(count => count > 0
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Item count cannot be 0 or below.[/]")));

            DateTime timeOfOrder = DateTime.Now;

            return new Order
            {
                Id = ManageKitchenOrdersController.NextOrderId+1,
                TableNumber = tableNumber,
                Items = itemCount,
                OrderedAt = timeOfOrder
            };
        }

        internal static void PrintFiglet()
        {
            var restaurantFiglet = new FigletText("The Code Bar & Restaurant")
                .Color(Color.Aqua);

            AnsiConsole.Write(restaurantFiglet);
        }

        internal static void Continue()
        {
            AnsiConsole.MarkupLine("[grey]Press 'Enter' to continue.[/]");
            Console.ReadKey();
        }

        internal static void WriteMessage(string message)
        {
            AnsiConsole.MarkupLine($"[white bold]{message}[/]");
        }

        internal static bool Confirm(string message)
        {
            return AnsiConsole.Confirm($"[white bold]{message}[/]");
        }

        internal static void Error(string message)
        {
            AnsiConsole.MarkupLine($"[red italic]{message}[/]");
        }

        internal static void ClearDisplay()
        {
            AnsiConsole.Clear();
        }
    }
}
