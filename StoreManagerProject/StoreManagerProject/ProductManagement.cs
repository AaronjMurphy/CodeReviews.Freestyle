using Spectre.Console;
using StoreManagerProject.Models;

namespace StoreManagerProject
{
    internal class ProductManagement
    {
        private static string[] TagManagementOptions = { "Add Tag", "Remove Tag", "Back to Product Management" };
        private const int RemovalHistoryLimit = 5;
        internal static void ViewProducts()
        {
            AnsiConsole.Clear();


            if (!StoreApp.Products.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no products to view![/]");
                StoreApp.Pause();
                return;
            }

            Table productsTable = new Table()
                .Title("[yellow bold]Products[/]")
                .AddColumn("[cyan bold]SKU[/]")
                .AddColumn("[cyan bold]Name[/]")
                .AddColumn("[cyan bold]Department[/]")
                .AddColumn("[cyan bold]Price[/]", col => col.RightAligned())
                .AddColumn("[cyan bold]Stock Quantity[/]", col => col.RightAligned())
                .AddColumn("[cyan bold]Tags[/]")
                .ShowRowSeparators();

            foreach (Product product in StoreApp.Products)
            {
                string allTags = string.Join(", ", product.Tags!);
                productsTable.AddRow(
                    Markup.Escape(product.Sku),
                    Markup.Escape(product.Name),
                    Markup.Escape(product.Department),
                    product.Price.ToString("C"),
                    product.StockQuantity.ToString(),
                    Markup.Escape(allTags));
            }

            AnsiConsole.Write(productsTable);
            StoreApp.Pause();
        }

        internal static void AddProduct()
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine("[yellow bold]Enter details for new product[/]\n");
            string sku = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan bold]Sku[/]:")
                .Validate(sku =>
                {

                    if (string.IsNullOrWhiteSpace(sku))
                    {
                        return ValidationResult.Error("[red italic]Sku is required![/]");
                    }

                    if (StoreApp.ProductsBySku.ContainsKey(sku))
                    {
                        return ValidationResult.Error($"[red italic]Sku {Markup.Escape(sku)} already exists![/]");
                    }

                    return ValidationResult.Success();

                }))
                .Trim();

            string name = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan bold]Name[/]:")
                .Validate(name => string.IsNullOrWhiteSpace(name)
                ? ValidationResult.Error("[red italic]Name is required![/]")
                : ValidationResult.Success()))
                .Trim();

            string department = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[cyan bold]Department[/]:")
                .AddChoices(StoreApp.Departments));

            decimal price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[cyan bold]Price[/]:")
                .Validate(price => price < 0
                ? ValidationResult.Error("[red italic]Price cannot be less than 0![/]")
                : ValidationResult.Success()));

            int stock = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan bold]Stock[/]:")
                .Validate(stock => stock < 0
                ? ValidationResult.Error("[red italic]Stock cannot be negative[/]")
                : ValidationResult.Success()));

            if (!AnsiConsole.Confirm("[white bold]Add this product to the catalogue?[/]"))
            {
                AnsiConsole.MarkupLine("[white bold]Addition cancelled![/]");
                StoreApp.Pause();
                return;
            }

            Product product = new Product
            {
                Sku = sku,
                Name = name,
                Department = department,
                Price = price,
                StockQuantity = stock,
                Tags = new HashSet<string>()
            };

            AddProductCollectionsSync(product);
            AnsiConsole.MarkupLine("[white bold]Product added![/]");
            StoreApp.Pause();
        }

        internal static void UpdateProduct()
        {
            AnsiConsole.Clear();

            if (!StoreApp.Products.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no products to update![/]");
                StoreApp.Pause();
                return;
            }

            string sku = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[yellow bold]Choose the Sku of the product you wish to update[/]:")
                .AddChoices(StoreApp.ProductsBySku.Keys));

            Product product = StoreApp.ProductsBySku[sku];

            product.Name = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan bold]Name[/]:")
                .Validate(name => string.IsNullOrWhiteSpace(name)
                ? ValidationResult.Error("[red italic]Name is required![/]")
                : ValidationResult.Success())
                .DefaultValue(product.Name))
                .Trim();

            product.Department = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[cyan bold]Department[/]:")
                .AddChoices(StoreApp.Departments)
                .DefaultValue(product.Department));

            product.Price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[cyan bold]Price[/]:")
                .Validate(price => price < 0
                ? ValidationResult.Error("[red italic]Price cannot be less than 0![/]")
                : ValidationResult.Success())
                .DefaultValue(product.Price));

            product.StockQuantity = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan bold]Stock[/]:")
                .Validate(stock => stock < 0
                ? ValidationResult.Error("[red italic]Stock cannot be negative[/]")
                : ValidationResult.Success())
                .DefaultValue(product.StockQuantity));

            AnsiConsole.MarkupLine("[white bold]Product updated successfully![/]");
            StoreApp.Pause();
        }

        internal static void RemoveProduct()
        {
            AnsiConsole.Clear();

            if (!StoreApp.Products.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no products to remove![/]");
                StoreApp.Pause();
                return;
            }

            string sku = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[yellow bold]Choose the Sku of the product you wish to remove[/]:")
                .AddChoices(StoreApp.ProductsBySku.Keys));

            Product product = StoreApp.ProductsBySku[sku];

            if (!AnsiConsole.Confirm($"[white bold]Are you sure you would like to remove {Markup.Escape(sku)}?[/]"))
            {
                AnsiConsole.MarkupLine("[white bold]Removal cancelled![/]");
                StoreApp.Pause();
                return;
            }

            RemoveProductCollectionsSync(product);
            AnsiConsole.MarkupLine("[white bold]Removal successful![/]");
            StoreApp.Pause();
        }

        internal static void FindProductBySku()
        {
            AnsiConsole.Clear();

            string productSku = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow bold]Enter an Sku to search for[/]:")
                    .Validate(sku => !string.IsNullOrWhiteSpace(sku)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red italic]Please enter a valid sku![/]")))
                .Trim();

            if (!StoreApp.ProductsBySku.ContainsKey(productSku))
            {
                AnsiConsole.MarkupLine("[red italic]There is no product with that sku![/]");
                StoreApp.Pause();
                return;
            }

            Product product = StoreApp.ProductsBySku[productSku];

            Table productDetails = new Table()
                .Title("[yellow bold]Product Details[/]")
                .AddColumn("")
                .AddColumn("")
                .AddRow("[cyan bold]Sku[/]", product.Sku)
                .AddRow("[cyan bold]Name[/]", product.Name)
                .AddRow("[cyan bold]Department[/]", product.Department)
                .AddRow("[cyan bold]Price[/]", product.Price.ToString())
                .AddRow("[cyan bold]Stock Quantity[/]", product.StockQuantity.ToString());

            AnsiConsole.Write(productDetails);
            StoreApp.Pause();
        }

        internal static void UndoRemoval()
        {
            AnsiConsole.Clear();

            if (!StoreApp.removedProducts.TryPeek(out Product? removedProduct))
            {
                AnsiConsole.MarkupLine("[red italic]There are no recently removed products![/]");
                StoreApp.Pause();
                return;
            }

            if (!AnsiConsole.Confirm($"[white bold]Would you like to restore {Markup.Escape(removedProduct.Sku)}({Markup.Escape(removedProduct.Name)})?[/]"))
            {
                AnsiConsole.MarkupLine("[white bold]Undo removal cancelled![/]");
                StoreApp.Pause();
                return;
            }

            StoreApp.removedProducts.Pop();
            AddProductCollectionsSync(removedProduct);
            AnsiConsole.MarkupLine("[white bold]Product restored successfully![/]");
            StoreApp.Pause();
        }

        internal static void ManageTags()
        {
            AnsiConsole.Clear();

            if (StoreApp.ProductsBySku.Count == 0)
            {
                AnsiConsole.MarkupLine("[red italic]There are no products![/]");
                StoreApp.Pause();
                return;
            }

            string sku = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[cyan bold]Choose product to manage tags[/]:")
                .AddChoices(StoreApp.ProductsBySku.Keys));
            bool backToProductManagement = false;

            Product product = StoreApp.ProductsBySku[sku];

            while (!backToProductManagement)
            {
                AnsiConsole.Clear();

                string tagMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"[cyan bold]What would you like to do with {Markup.Escape(product.Sku)}({Markup.Escape(product.Name)})?[/]")
                    .AddChoices(TagManagementOptions));

                switch (tagMenuChoice)
                {
                    case "Add Tag":
                        AddTag(product);
                        break;
                    case "Remove Tag":
                        RemoveTag(product);
                        break;
                    case "Back to Product Management":
                        backToProductManagement = true;
                        break;
                }
            }
        }

        internal static void AddTag(Product product)
        {
            AnsiConsole.Clear();

            string tag;

            tag = AnsiConsole.Prompt(
                new TextPrompt<string>($"[yellow bold]Enter a tag for {Markup.Escape(product.Sku)}({Markup.Escape(product.Name)})[/]:")
                .Validate(tag => !string.IsNullOrWhiteSpace(tag)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red italic]Please enter a valid tag![/]")))
                .Trim();

            if (product.Tags.Contains(tag))
            {
                AnsiConsole.MarkupLine("[red italic]That tag is already assigned to this product![/]");
                StoreApp.Pause();
                return;
            }

            product.Tags.Add(tag);
            AnsiConsole.MarkupLine("[white bold]Tag added successfully![/]");
            StoreApp.Pause();
        }

        internal static void RemoveTag(Product product)
        {
            AnsiConsole.Clear();

            if (product.Tags.Count == 0)
            {
                AnsiConsole.MarkupLine("[red italic]There are no tags to remove![/]");
                StoreApp.Pause();
                return;
            }

            string tagToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[yellow bold]Which tag would you like to remove?[/]")
                .AddChoices(product.Tags));

            if (!AnsiConsole.Confirm($"[white bold]Are you sure you would like to remove this tag?[/]"))
            {
                AnsiConsole.MarkupLine("[white bold]Tag removal cancelled.[/]");
                StoreApp.Pause();
                return;
            }

            product.Tags.Remove(tagToDelete);
            AnsiConsole.MarkupLine($"[white bold]Tag {Markup.Escape(tagToDelete)} removed successfully![/]");
            StoreApp.Pause();
        }

        private static void AddProductCollectionsSync(Product product)
        {
            StoreApp.Products.Add(product);
            StoreApp.ProductsBySku.Add(product.Sku, product);
        }

        private static void RemoveProductCollectionsSync(Product product)
        {
            StoreApp.Products.Remove(product);
            StoreApp.ProductsBySku.Remove(product.Sku);
            RemoveWithLimitHistory(product);
        }

        internal static void SearchForProductWithTag()
        {
            AnsiConsole.Clear();

            string searchTag = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow bold]Enter a tag to search for a product[/]:")
                    .Validate(tag => string.IsNullOrWhiteSpace(tag)
                    ? ValidationResult.Error("Please enter a valid tag!")
                    : ValidationResult.Success()))
                .Trim();

            AnsiConsole.Clear();

            List<Product> matchingProducts = StoreApp.Products.Where(prod => prod.Tags.Contains(searchTag)).ToList();

            if (!matchingProducts.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no products with that tag![/]");
                StoreApp.Pause();
                return;
            }

            Table productsWithTag = new Table()
                .Title($"[yellow bold]Products With \"{Markup.Escape(searchTag)}\" Tag[/]")
                .AddColumn("[cyan bold]Sku[/]")
                .AddColumn("[cyan bold]Name[/]")
                .AddColumn("[cyan bold]Department[/]")
                .AddColumn("[cyan bold]Price[/]")
                .AddColumn("[cyan bold]StockQuantity[/]");

            foreach (Product product in matchingProducts)
            {
                productsWithTag.AddRow(
                    Markup.Escape(product.Sku),
                    Markup.Escape(product.Name),
                    Markup.Escape(product.Department),
                    product.Price.ToString("C"),
                    product.StockQuantity.ToString());
            }

            AnsiConsole.Write(productsWithTag);
            StoreApp.Pause();
        }

        internal static void ViewLowStockProducts()
        {
            const int lowStockThreshold = 10;
            AnsiConsole.Clear();

            var lowStockProducts = StoreApp.Products
                .Where(p => p.StockQuantity <= lowStockThreshold)
                .GroupBy(p => p.Department);

            if (!lowStockProducts.Any())
            {
                AnsiConsole.MarkupLine("[red italic]There are no low stock products to report![/]");
                StoreApp.Pause();
                return;
            }

            foreach(var department in lowStockProducts)
            {
                Table lowStockProductsTable = new Table()
                    .Title($"[yellow bold]Low Stock Products in {Markup.Escape(department.Key)}[/]")
                    .AddColumn("[cyan bold]Sku[/]")
                    .AddColumn("[cyan bold]Name[/]")
                    .AddColumn("[cyan bold]Price[/]")
                    .AddColumn("[cyan bold]Stock Quantity[/]");

                foreach (Product product in department)
                {
                    lowStockProductsTable.AddRow(
                        Markup.Escape(product.Sku),
                        Markup.Escape(product.Name),
                        product.Price.ToString("C"),
                        product.StockQuantity.ToString());
                }

                AnsiConsole.Write(lowStockProductsTable);
            }

            StoreApp.Pause();
        }

        private static void RemoveWithLimitHistory(Product product)
        {
            StoreApp.removedProducts.Push(product);

            if (StoreApp.removedProducts.Count <= RemovalHistoryLimit)
            {
                return;
            }

            Product[] newestToOldestProducts = StoreApp.removedProducts.ToArray();
            StoreApp.removedProducts.Clear();

            for (int index = RemovalHistoryLimit - 1; index >= 0; index--)
            {
                StoreApp.removedProducts.Push(newestToOldestProducts[index]);
            }
        }

    }
}
