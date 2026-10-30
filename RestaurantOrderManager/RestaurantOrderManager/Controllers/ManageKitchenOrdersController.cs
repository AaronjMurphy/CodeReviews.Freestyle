using RestaurantOrderManager.Data;
using RestaurantOrderManager.Models;
using RestaurantOrderManager.UI;

namespace RestaurantOrderManager.Controllers
{
    internal class ManageKitchenOrdersController
    {
        internal static Queue<Order> OutstandingOrders = new Queue<Order>(OrderSeeder.SeedOrders());
        internal static int NextOrderId = GetMaxOrderId();
        internal static List<Order> ProcessedOrders = new List<Order>();

        internal static void ViewAllOutstandingOrders()
        {
            if (!OutstandingOrders.Any())
            {
                UserInput.Error("There are no currently outstanding orders.");
                UserInput.Continue();
                return;
            }

            TablePrinter.PrintOutstandingOrdersTable(OutstandingOrders);
            UserInput.Continue();
        }

        internal static void AddOrder()
        {
            Order newOrder = UserInput.GetNewOrderDetails();
            if (!UserInput.Confirm("Would you like to add this order?"))
            {
                UserInput.WriteMessage("Order was not added.");
                UserInput.Continue();
                return;
            }

            OutstandingOrders.Enqueue(newOrder);
            NextOrderId++;
            UserInput.WriteMessage("Order added successfully.");
            UserInput.Continue();
        }

        internal static void ProcessOrder()
        {
            if (!OutstandingOrders.Any())
            {
                UserInput.Error("There are no outstanding orders to process.");
                UserInput.Continue();
                return;
            }

           
            while (OutstandingOrders.TryPeek(out Order? nextOrder))
            {
                UserInput.ClearDisplay();
                if (!UserInput.Confirm($"Would you like to process order no. {nextOrder.Id}?"))
                {
                    UserInput.WriteMessage("Order processing cancelled.");
                    UserInput.Continue();
                    return;
                }
                OutstandingOrders.TryDequeue(out _);
                ExecuteOrderProcessing(nextOrder);
                UserInput.WriteMessage($"Order no. {nextOrder.Id} has been processed.");
                Thread.Sleep(500);
            }
            UserInput.ClearDisplay();
            UserInput.WriteMessage("All orders have been processed.");
            UserInput.Continue();
        }

        internal static void ExecuteOrderProcessing(Order nextOrder)
        {
            nextOrder.ProcessedAt = DateTime.Now;
            ProcessedOrders.Add(nextOrder);
        }

        internal static void ViewOrderHistory()
        {
            if (!ProcessedOrders.Any())
            {
                UserInput.Error("There are no processed orders in the system.");
                UserInput.Continue();
                return;
            }

            TablePrinter.PrintProcessedOrderHistory(ProcessedOrders);
            UserInput.Continue();
        }

        private static int GetMaxOrderId()
        {
            if (!OutstandingOrders.Any())
            {
                return 0;
            }

            int maxId = OutstandingOrders.Max(order => order.Id);

            return maxId;
        }
    }
}