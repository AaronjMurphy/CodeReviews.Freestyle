using Spectre.Console;
using StoreManagerProject.Models;

namespace StoreManagerProject
{
    internal class OrderManagement
    {
        internal static void ViewPendingOrders()
        {
            AnsiConsole.Clear();
            if (!StoreApp.pendingOrders.TryPeek(out _))
            {
                AnsiConsole.MarkupLine("[red italic]There are no pending orders![/]");
                StoreApp.Pause();
                return;
            }

            Table pendingOrderTable = new Table()
                .Title("[yellow bold]Pending Orders[/]")
                .AddColumn("[cyan bold]Id[/]")
                .AddColumn("[cyan bold]Customer Name[/]")
                .AddColumn("[cyan bold]Item Count[/]")
                .AddColumn("[cyan bold]Date Created[/]")
                .ShowRowSeparators();

            foreach (Order order in StoreApp.pendingOrders)
            {
                pendingOrderTable.AddRow(
                    order.Id.ToString(),
                    Markup.Escape(order.CustomerName),
                    order.ItemCount.ToString(),
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            }

            AnsiConsole.Write(pendingOrderTable);
            StoreApp.Pause();
        }

        internal static void CreateOrder()
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine("[yellow bold]Enter order details[/]\n");

            string customerName = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan bold]Enter the customer name[/]:")
                    .Validate(name => !string.IsNullOrWhiteSpace(name)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red italic]Name is required![/]")))
                .Trim();

            int itemCount = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan bold]Enter the item count[/]:")
                .Validate(count => count > 0
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Item count must be positive![/]")));

            Order newOrder = new Order
            {
                Id = StoreApp._nextOrderNumber,
                CustomerName = customerName,
                ItemCount = itemCount,
                CreatedAt = DateTime.Now
            };

            if (!AnsiConsole.Confirm("[white bold]Add this order to the queue?[/]"))
            {
                AnsiConsole.MarkupLine("[white bold]Order creation cancelled![/]");
                StoreApp.Pause();
                return;
            }
            StoreApp._nextOrderNumber++;
            StoreApp.pendingOrders.Enqueue(newOrder);
            AnsiConsole.MarkupLine("[white bold]Order added successfully![/]");
            StoreApp.Pause();
        }

        internal static void ProcessNextOrder()
        {
            AnsiConsole.Clear();

            if (!StoreApp.pendingOrders.TryPeek(out Order? order))
            {
                AnsiConsole.MarkupLine("[red italic]There are no orders to process![/]");
                StoreApp.Pause();
                return;
            }

            do
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine(
                    $"ID: {order.Id}\nCustomerName: {Markup.Escape(order.CustomerName)}\nItem Count: {order.ItemCount}\nDate Created: {order.CreatedAt.ToString("yyyy-MM-dd HH:mm")}\n"
                    );

                if (!AnsiConsole.Confirm("[white bold]Would you like to process this order?[/]\n"))
                {
                    AnsiConsole.MarkupLine("[white bold]Order processing cancelled![/]");
                    break;
                }

                order.ProcessedAt = DateTime.Now;
                StoreApp.pendingOrders.Dequeue();
                StoreApp.CompletedOrderHistory.Add(order);
                AnsiConsole.MarkupLine($"[white bold]Order {order.Id} processed![/]");
                Thread.Sleep(800);

            } while (StoreApp.pendingOrders.TryPeek(out order));

            if (StoreApp.pendingOrders.Count == 0)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[white bold]All orders have been processed![/]");
            }
            StoreApp.Pause();
        }

        internal static void ViewOrderHistory()
        {
            AnsiConsole.Clear();

            if (StoreApp.CompletedOrderHistory.Count == 0)
            {
                AnsiConsole.MarkupLine("[red italic]There are no completed orders.[/]");
                StoreApp.Pause();
                return;
            }

            Table orderHistory = new Table()
                .Title("[yellow bold]Completed Order History[/]")
                .AddColumn("[cyan bold]Order Id[/]")
                .AddColumn("[cyan bold]Customer Name[/]")
                .AddColumn("[cyan bold]Item Count[/]")
                .AddColumn("[cyan bold]Date Ordered[/]")
                .AddColumn("[cyan bold]Processed At[/]");

            foreach (Order order in StoreApp.CompletedOrderHistory)
            {
                orderHistory.AddRow(
                    order.Id.ToString(),
                    Markup.Escape(order.CustomerName),
                    order.ItemCount.ToString(),
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    order.ProcessedAt.ToString("yyyy-MM-dd HH:mm"));
            }

            AnsiConsole.Write(orderHistory);
            StoreApp.Pause();
        }

    }
}
