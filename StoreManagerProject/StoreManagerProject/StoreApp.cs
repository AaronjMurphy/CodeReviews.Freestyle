using Spectre.Console;
using StoreManagerProject.Data;
using StoreManagerProject.Models;

namespace StoreManagerProject
{
    internal class StoreApp
    {
        private static readonly string[] MenuOptions = { "View Departments", "Manage Products", "Manage Orders", "Store Summary", "Exit" };
        internal static readonly string[] Departments = { "Electronics", "Groceries", "Clothing", "Furniture", "Toys & Gifts" };
        private static readonly string[] ManageProductsOptions = { "View Products", "Add Product", "Update Product", "Remove Product","Undo Product Removal",
                                                                   "Find Product", "Manage Product Tags", "Search Product With Tag", "View Low Stock Products" 
                                                                  ,"Return to Main Menu"};
        private static readonly string[] ManageOrderOptions = { "View Pending Orders", "Create Order", "Process Next Order", "View Order History", "Return to Main Menu" };
        public static List<Product> Products = ProductListSeeder.GetProductData();
        public static Dictionary<string, Product> ProductsBySku = Products.ToDictionary(product => product.Sku, StringComparer.OrdinalIgnoreCase);
        public static Stack<Product> removedProducts = new Stack<Product>();
        public static int _nextOrderNumber = 101;
        public static Queue<Order> pendingOrders = OrderQueueSeeder.GetOrderdata();
        public static List<Order> CompletedOrderHistory = new List<Order>();


        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                AnsiConsole.Clear();

                ShowHeader();

                string menuChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow bold]Choose an option:[/]")
                            .AddChoices(MenuOptions));

                switch (menuChoice)
                {
                    case "View Departments":
                        ViewDepartments();
                        break;
                    case "Manage Products":
                        ManageProducts();
                        break;
                    case "Manage Orders":
                        ManageOrders();
                        break;
                    case "Store Summary":
                        ViewStoreSummary();
                        break;
                    case "Exit":
                        isRunning = false;
                        break;
                }
            }
        }

        private static void ViewDepartments()
        {
            AnsiConsole.Clear();

            Table departmentsTable = new Table()
                .Title("[yellow bold]Departments[/]")
                .AddColumn("[cyan bold]Number[/]", col => col.RightAligned())
                .AddColumn("[cyan bold]Name[/]");

            if (!Departments.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no departments to view![/]");
                Pause();
                return;
            }

            for (int index = 0; index < Departments.Length; index++)
            {
                departmentsTable.AddRow(
                    $"{index + 1}",
                    Markup.Escape(Departments[index]));
            }

            AnsiConsole.Write(departmentsTable);
            Pause();
        }

        private static void ManageProducts()
        {
            bool returnToMainMenu = false;

            while (!returnToMainMenu)
            {
                AnsiConsole.Clear();

                string productMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow bold]What would you like to do?[/]")
                        .AddChoices(ManageProductsOptions));

                switch (productMenuChoice)
                {
                    case "View Products":
                        ProductManagement.ViewProducts();
                        break;
                    case "Add Product":
                        ProductManagement.AddProduct();
                        break;
                    case "Update Product":
                        ProductManagement.UpdateProduct();
                        break;
                    case "Remove Product":
                        ProductManagement.RemoveProduct();
                        break;
                    case "Undo Product Removal":
                        ProductManagement.UndoRemoval();
                        break;
                    case "Find Product":
                        ProductManagement.FindProductBySku();
                        break;
                    case "Manage Product Tags":
                        ProductManagement.ManageTags();
                        break;
                    case "Search Product With Tag":
                        ProductManagement.SearchForProductWithTag();
                        break;
                    case "View Low Stock Products":
                        ProductManagement.ViewLowStockProducts();
                        break;
                    case "Return to Main Menu":
                        returnToMainMenu = true;
                        break;
                }
            }

        }

        private static void ManageOrders()
        {
            bool returnToMainMenu = false;

            while (!returnToMainMenu)
            {
                AnsiConsole.Clear();

                string orderMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[yellow bold]What would you like to do?[/]")
                    .AddChoices(ManageOrderOptions));

                switch (orderMenuChoice)
                {
                    case "View Pending Orders":
                        OrderManagement.ViewPendingOrders();
                        break;
                    case "Create Order":
                        OrderManagement.CreateOrder();
                        break;
                    case "Process Next Order":
                        OrderManagement.ProcessNextOrder();
                        break;
                    case "View Order History":
                        OrderManagement.ViewOrderHistory();
                        break;
                    case "Return to Main Menu":
                        returnToMainMenu = true;
                        break;
                }
            }
        }

        private static void ViewStoreSummary()
        {
            AnsiConsole.Clear();

            HashSet<string> allTags = GetUniqueStoreTags();

            var totals = new Table()
                .Title("[yellow bold]Store Summary[/]")
                .AddColumn("[cyan bold]Measure[/]")
                .AddColumn("[cyan bold]Total[/]")
                .AddRow("Departments", Departments.Length.ToString())
                .AddRow("Active Products", Products.Count.ToString())
                .AddRow("Unique Store Tags", allTags.Count.ToString())
                .AddRow("Removed Products", removedProducts.Count.ToString())
                .AddRow("Pending Orders", pendingOrders.Count.ToString());

            AnsiConsole.Write(totals);
            AnsiConsole.WriteLine();

            var productsByDepartment = new Table()
                .Title("[yellow bold]Products by Department[/]")
                .AddColumn("[cyan bold]Department[/]")
                .AddColumn("[cyan bold]Products[/]");

            foreach (string department in Departments)
            {
                int productCount = Products.Count(
                    product => product.Department == department);

                productsByDepartment.AddRow(
                    Markup.Escape(department),
                    productCount.ToString());
            }

            AnsiConsole.Write(productsByDepartment);
            Pause();
        }

        private static HashSet<string> GetUniqueStoreTags()
        {
            var storeTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Product product in Products)
            {
                storeTags.UnionWith(product.Tags);
            }

            return storeTags;
        }

        private static void ShowHeader()
        {
            var figlet = new FigletText("Store Manager")
            {
                Justification = Justify.Center,
                Color = Color.Aqua,  
            };

            AnsiConsole.Write(figlet);
        }

        public static void Pause()
        {
            AnsiConsole.MarkupLine("[grey bold]Press Enter to continue.[/]");
            Console.ReadLine();
        }
    }
}
