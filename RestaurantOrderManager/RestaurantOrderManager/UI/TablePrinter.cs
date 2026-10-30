using RestaurantOrderManager.Models;
using Spectre.Console;
namespace RestaurantOrderManager.UI
{
    internal class TablePrinter
    {
        internal static void PrintCategoriesTable(string[] categories)
        {
            Table categoryTable = new Table()
                .Title("[white bold]Categories[/]")
                .AddColumn("[cyan bold]Number[/]")
                .AddColumn("[cyan bold]Category[/]");

            for (int index = 0; index < categories.Length; index++)
            {
                categoryTable.AddRow(
                    (index + 1).ToString(),
                    Markup.Escape(categories[index]));
            }

            AnsiConsole.Write(categoryTable);
        }

        internal static void PrintMenuItemsTable(List<MenuItem> menuItems)
        {
            List<MenuItem> sortedMenuItems = menuItems.OrderBy(item => item.Category).ThenBy(item => item.Name).ToList();

            Table menuItemsTable = new Table()
                .Title("[white bold]Menu Items[/]")
                .AddColumn("[cyan bold]ID[/]")
                .AddColumn("[cyan bold]Code[/]")
                .AddColumn("[cyan bold]Name[/]")
                .AddColumn("[cyan bold]Category[/]")
                .AddColumn("[cyan bold]Price[/]")
                .AddColumn("[cyan bold]Dietary Labels[/]")
                .ShowRowSeparators();

            foreach (MenuItem item in sortedMenuItems)
            {
                menuItemsTable.AddRow(
                    item.Id.ToString(),
                    Markup.Escape(item.Code),
                    Markup.Escape(item.Name),
                    Markup.Escape(item.Category),
                    item.Price.ToString(),
                    Markup.Escape(item.DietaryLabels != null ? string.Join(", ", item.DietaryLabels) : string.Empty));
            }

            AnsiConsole.Write(menuItemsTable);
        }

        internal static void PrintOutstandingOrdersTable(Queue<Order> outstandingOrders)
        {
            Table outstandingOrdersTable = new Table()
                .Title("[white bold]Outstanding Orders[/]")
                .AddColumn("[cyan bold]Id[/]")
                .AddColumn("[cyan bold]Table Number[/]")
                .AddColumn("[cyan bold]No. of Items[/]")
                .AddColumn("[cyan bold]CreatedAt[/]")
                .ShowRowSeparators();

            foreach (Order order in outstandingOrders)
            {
                outstandingOrdersTable.AddRow(
                    order.Id.ToString(),
                    order.TableNumber.ToString(),
                    order.Items.ToString(),
                    order.OrderedAt.ToString());
            }

            AnsiConsole.Write(outstandingOrdersTable);
        }

        internal static void PrintProcessedOrderHistory(List<Order> orderHistory)
        {
            Table processedOrdersTable = new Table()
                .Title("[white bold]Processed Order History[/]")
                .AddColumn("[cyan bold]Id[/]")
                .AddColumn("[cyan bold]Table No.[/]")
                .AddColumn("[cyan bold]Item Count[/]")
                .AddColumn("[cyan bold]Time Ordered[/]")
                .AddColumn("[cyan bold]Time Processed[/]");

            foreach (Order order in orderHistory)
            {
                processedOrdersTable.AddRow(
                    order.Id.ToString(),
                    order.TableNumber.ToString(),
                    order.Items.ToString(),
                    order.OrderedAt.ToString("yyyy-MM-dd HH:mm"),
                    order.ProcessedAt.ToString("yyyy-MM-dd HH:mm")
                    );
            }

            AnsiConsole.Write(processedOrdersTable);
        }

        internal static void PrintRestaurantSummary(string[] categories, List<MenuItem> menuItems, Stack<MenuItem> recoverableMenuItems, 
            Queue<Order> outstandingOrders, List<Order> processedOrders)
        {
            PrintCategoriesWithItemCount(categories, menuItems);
            PrintGeneralSummary(categories, menuItems, recoverableMenuItems, outstandingOrders, processedOrders);
        }

        private static void PrintCategoriesWithItemCount(string[] categories, List<MenuItem> menuItems)
        {
            Table categoriesWithItemCountTable = new Table()
                .Title("[white bold]Categories With Item Count[/]")
                .AddColumn("[cyan bold]Category[/]")
                .AddColumn("[cyan bold]Item Count[/]");

            foreach (string category in categories)
            {
                int itemCount = menuItems.Count(item => item.Category == category);

                categoriesWithItemCountTable.AddRow(
                    Markup.Escape(category),
                    itemCount.ToString()
                    );
            }

            AnsiConsole.Write(categoriesWithItemCountTable);
        }

        private static void PrintGeneralSummary(string[] categories, List<MenuItem> menuItems, Stack<MenuItem> recoverableMenuItems,
            Queue<Order> outstandingOrders, List<Order> processedOrders)
        {
            Table generalSummaryTable = new Table()
                .Title("[white bold]General Summary[/]")
                .AddColumn("")
                .AddColumn("");

            generalSummaryTable
                .AddRow("[cyan bold]Categories[/]", categories.Length.ToString())
                .AddRow("[cyan bold]Menu Items[/]", menuItems.Count.ToString())
                .AddRow("[cyan bold]Recoverable Menu Items[/]", recoverableMenuItems.Count.ToString())
                .AddRow("[cyan bold]Outstanding Orders[/]", outstandingOrders.Count.ToString())
                .AddRow("[cyan bold]Processed Orders[/]", processedOrders.Count.ToString());

            AnsiConsole.Write(generalSummaryTable);
        }
    }
}
