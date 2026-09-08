using Spectre.Console;

namespace TCSA.Study.Linq.Printing
{
    public static class ReportPrinter
    {
        public static void PrintPortfolioSummary(List<Stock> stocks)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var allTrades = stocks.SelectMany(stock => stock.Trades).ToList();

                var summary = new PortfolioSummaryRow()
                {
                    TotalStocks = stocks.Count(),
                    TotalTrades = allTrades.LongCount(),
                    TotalQuantity = allTrades.Sum(trade => trade.Quantity),
                    TotalTradeValue = allTrades.Sum(trade => trade.Quantity * trade.Price),
                    AverageTradePrice = allTrades.Average(trade => trade.Price),
                    LowestTradePrice = allTrades.Min(trade => trade.Price),
                    HighestTradePrice = allTrades.Max(trade => trade.Price),
                    EarliestTradeDate = allTrades.Min(trade => trade.Date),
                    LatestTradeDate = allTrades.Max(trade => trade.Date)

                };

                var portfolioSummaryRows = new Rows(
                    new Markup($"[cyan bold]Total Stocks[/]: {summary.TotalStocks}"),
                    new Markup($"[cyan bold]Total Trades[/]: {summary.TotalTrades}"),
                    new Markup($"[cyan bold]Total Quantity Traded[/]: {summary.TotalQuantity}"),
                    new Markup($"[cyan bold]Total Trade Value[/]: {summary.TotalTradeValue:C}"),
                    new Markup($"[cyan bold]Average Trade Price[/]: {summary.AverageTradePrice:C}"),
                    new Markup($"[cyan bold]Lowest Trade Price[/]: {summary.LowestTradePrice:C}"),
                    new Markup($"[cyan bold]Highest Trade Price[/]: {summary.HighestTradePrice:C}"),
                    new Markup($"[cyan bold]Earliest Trade Date[/]: {summary.EarliestTradeDate:yyyy-MM-dd}"),
                    new Markup($"[cyan bold]Most Recent Trade Date[/]: {summary.LatestTradeDate:yyyy-MM-dd}")
                    );

                var portfolioSummaryPanel = new Panel(portfolioSummaryRows)
                    .Header("[yellow bold]Portfolio Summary[/]")
                    .Border(BoxBorder.Rounded);

                AnsiConsole.Write(portfolioSummaryPanel);

                if (ExitReport() == "exit")
                {
                    break;
                }

                else Console.Clear();

            }

            Console.Clear();
        }

        public static void reportSectors(List<Stock> stocks)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var sectorChoices = stocks.Select(stock => stock.Sector).Distinct().ToList();

                var selectedSector = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("[yellow bold]Choose a sector.[/]")
                    .AddChoices(sectorChoices));

                var orderedStocks = stocks.Where(stock => stock.Sector == selectedSector).OrderBy(stock => stock.CompanyName);

                Table stocksBySectorTable = new Table()
                    .Border(TableBorder.Rounded)
                    .Title($"[yellow bold]{selectedSector}[/]");

                stocksBySectorTable.AddColumn("[cyan bold]Company Name[/]");
                stocksBySectorTable.AddColumn("[cyan bold]Symbol[/]");
                stocksBySectorTable.AddColumn("[cyan bold]Trade Quantity[/]");

                foreach (var stock in orderedStocks)
                {
                    stocksBySectorTable.AddRow(
                        stock.CompanyName,
                        $"[yellow]{stock.Symbol}[/]",
                        stock.Trades.Count().ToString()
                    );
                }

                AnsiConsole.Write(stocksBySectorTable);

                if (ExitReport() == "exit")
                {
                    break;
                }

                else Console.Clear();
            }

            Console.Clear();
        }

        public static void reportTradesBySymbol(List<Stock> stocks, List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();

                var stockSymbolChoices = stocks.Select(stock => stock.Symbol).Distinct().ToList();

                var selectedSymbol = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("[yellow bold]Choose a stock symbol.[/]")
                    .AddChoices(stockSymbolChoices));

                var orderedTrades = trades.Where(trade => trade.Symbol == selectedSymbol).OrderByDescending(trade => trade.Date);

                Table tradesBySymbolTable = new Table()
                    .Border(TableBorder.Rounded)
                    .Title($"[yellow bold]Trades for {selectedSymbol}[/]");

                tradesBySymbolTable.AddColumn("[cyan bold]ID[/]");
                tradesBySymbolTable.AddColumn("[cyan bold]Company Name[/]");
                tradesBySymbolTable.AddColumn("[cyan bold]Date[/]");
                tradesBySymbolTable.AddColumn("[cyan bold]Price[/]");
                tradesBySymbolTable.AddColumn("[cyan bold]Quantity[/]");
                tradesBySymbolTable.AddColumn("[cyan bold]Type[/]");

                foreach (var trade in orderedTrades)
                {
                    var parentStock = stocks.Single(stock => stock.Symbol == trade.Symbol);

                    tradesBySymbolTable.AddRow(
                        trade.Id.ToString(),
                        parentStock.CompanyName,
                        $"{trade.Date:yyyy-MM-dd}",
                        $"{trade.Price:C}",
                        trade.Quantity.ToString(),
                        trade.Type.ToString()
                        );
                }

                AnsiConsole.Write(tradesBySymbolTable);

                if (ExitReport() == "exit")
                {
                    break;
                }

            }

            AnsiConsole.Clear();

        }

        public static void reportHighVolumeTrades(List<Stock> stocks, List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var minimumQuantity = AnsiConsole.Ask<int>("[yellow bold]Enter a minimum quantity[/]");

                var orderedTradesBySymbolThenQuantity = trades
                    .Where(trade => trade.Quantity >= minimumQuantity)
                    .OrderBy(trade => trade.Symbol)
                    .ThenByDescending(trade => trade.Quantity)
                    .ToList();

                if (orderedTradesBySymbolThenQuantity.Any())
                {
                    Table highQuantityTradesTable = new Table()
                                    .Border(TableBorder.Rounded)
                                    .Title($"Trades above {minimumQuantity}");

                    highQuantityTradesTable.AddColumn("[cyan bold]ID[/]");
                    highQuantityTradesTable.AddColumn("[cyan bold]Symbol[/]");
                    highQuantityTradesTable.AddColumn("[cyan bold]Date[/]");
                    highQuantityTradesTable.AddColumn("[cyan bold]Price[/]");
                    highQuantityTradesTable.AddColumn("[cyan bold]Quantity[/]");
                    highQuantityTradesTable.AddColumn("[cyan bold]Type[/]");

                    foreach (var trade in orderedTradesBySymbolThenQuantity)
                    {
                        highQuantityTradesTable.AddRow(
                            trade.Id.ToString(),
                            $"[yellow]{trade.Symbol}[/]",
                            $"{trade.Date:yyyy-MM-dd}",
                            $"{trade.Price:C}",
                            trade.Quantity.ToString(),
                            trade.Type.ToString()
                            );
                    }

                    AnsiConsole.Write(highQuantityTradesTable);
                }

                else
                {
                    AnsiConsole.WriteLine("There are no trades that meet that quantity.");
                }

                if (ExitReport() == "exit")
                {
                    break;
                }
            }

            AnsiConsole.Clear();
        }

        public static void reportStockActivity(List<Stock> stocks)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var reportRows = stocks
                .Select(stock => {

                    var highestTrade = stock.Trades.OrderByDescending(trade => trade.Price).FirstOrDefault();

                    var latestTrade = stock.Trades.OrderByDescending(trade => trade.Date).FirstOrDefault();
                    ;

                    return new StockActivityReportRow
                    {
                        Symbol = stock.Symbol,
                        CompanyName = stock.CompanyName,
                        Sector = stock.Sector,
                        TradeCount = stock.Trades.Count(),
                        TotalQuantity = stock.Trades.Sum(trade => trade.Quantity),
                        TotalValue = stock.Trades.Sum(trade => trade.Price * trade.Quantity),

                        HighestTradePrice = highestTrade != null ? highestTrade.Price : 0,
                        LatestTradeDate = latestTrade != null ? latestTrade.Date : DateTime.MinValue
                    };

                })
                .OrderBy(row => row.Sector)
                .ThenBy(row => row.Symbol)
                .ToList();

                Table stockActivityTable = new Table()
                    .Title("[yellow bold]Stock Activity Report[/]")
                    .Border(TableBorder.Rounded);

                stockActivityTable.AddColumn("[cyan bold]Symbol[/]");
                stockActivityTable.AddColumn("[cyan bold]Company Name[/]");
                stockActivityTable.AddColumn("[cyan bold]Sector[/]");
                stockActivityTable.AddColumn("[cyan bold]Trade Count[/]");
                stockActivityTable.AddColumn("[cyan bold]Total Quantity[/]");
                stockActivityTable.AddColumn("[cyan bold]Total Value[/]");
                stockActivityTable.AddColumn("[cyan bold]Highest Trade Price[/]");
                stockActivityTable.AddColumn("[cyan bold]Latest Trade Date[/]");

                foreach (var row in reportRows)
                {
                    stockActivityTable.AddRow(
                        $"[yellow]{row.Symbol}[/]",
                        row.CompanyName,
                        row.Sector,
                        row.TradeCount.ToString(),
                        row.TotalQuantity.ToString(),
                        $"{row.TotalValue:C}",
                        $"{row.HighestTradePrice:C}",
                        $"{row.LatestTradeDate:yyyy-MM-dd}");
                }

                if (reportRows.Any())
                {
                    AnsiConsole.Write(stockActivityTable);
                }
                else
                {
                    AnsiConsole.WriteLine("There is no stock data to display.");
                }

                if (ExitReport() == "exit")
                {
                    break;
                }
            }

            AnsiConsole.Clear();
        }

        public static void reportUniqueValues(List<Stock> stocks)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var uniqueValuesRows = new UniqueValues
                {
                    Sectors = string.Join(", ", stocks.Select(stock => stock.Sector).Distinct()),
                    StockSymbols = string.Join(", ", stocks.Select(stock => stock.Symbol).Distinct()),
                    TradeSymbols = string.Join(", ", stocks.SelectMany(stock => stock.Trades).Select(trade => trade.Symbol).Distinct()),
                    TradeTypes = string.Join(", ", stocks.SelectMany(stock => stock.Trades).Select(trade => trade.Type).Distinct())
                };

                Table uniqueValuesTable = new Table()
                    .Title("[yellow bold]Unique Values[/]")
                    .Border(TableBorder.Rounded);

                uniqueValuesTable.AddColumn("[yellow bold]Category[/]");
                uniqueValuesTable.AddColumn("[yellow bold]Unique Values Found[/]");


                uniqueValuesTable.AddRow("[cyan bold]Unique Sectors[/]", uniqueValuesRows.Sectors);
                uniqueValuesTable.AddRow("[cyan bold]Unique Stock Symbols[/]", uniqueValuesRows.StockSymbols);
                uniqueValuesTable.AddRow("[cyan bold]Unique Trade Symbols[/]", uniqueValuesRows.TradeSymbols);
                uniqueValuesTable.AddRow("[cyan bold]Unique Trade Types[/]", uniqueValuesRows.TradeTypes);


                AnsiConsole.Write(uniqueValuesTable);

                if (ExitReport() == "exit")
                {
                    break;
                }
            }

            AnsiConsole.Clear();
        }

        public static void reportDataQuality(List<Stock> stocks, List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();
                bool allStocksHaveTrades = stocks.Select(stock => stock.Trades).All(trade => trade.Count() is not 0);

                AnsiConsole.MarkupLine("[yellow bold underline]Data Quality Report[/]\n");

                if (allStocksHaveTrades)
                {
                    AnsiConsole.MarkupLine("[green bold]All stocks have trades![/]\n");
                }
                else AnsiConsole.MarkupLine("[red bold]Not all stocks have trades!![/]\n");

                var highestQuantityTrade = trades.OrderByDescending(trade => trade.Quantity).First();

                AnsiConsole.MarkupLine($"The trade with the highest quantity is from [yellow bold]{highestQuantityTrade.Symbol}[/] with a quantity of [yellow bold]{highestQuantityTrade.Quantity}[/] bringing its total value to [bold yellow]{(highestQuantityTrade.Quantity * highestQuantityTrade.Price):C}[/].\n");

                var chosenValue = AnsiConsole.Ask<decimal>("[bold]Enter a minimum value(trade price x quantity) to search for: [/]");
                Table tradesAboveChosenValueTable = new Table()
                    .Title($"[yellow bold]Trades with a value at or above [/][blue bold]{chosenValue:C}[/]")
                    .Border(TableBorder.Rounded);

                tradesAboveChosenValueTable.AddColumn("[cyan bold]Id[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Symbol[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Date[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Price[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Quantity[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Type[/]");
                tradesAboveChosenValueTable.AddColumn("[cyan bold]Value[/]");



                var tradesAboveChosenValue = trades.Where(trade => trade.Price * trade.Quantity >= chosenValue);

                if (tradesAboveChosenValue.Any())
                {
                    AnsiConsole.MarkupLine("[green bold]Matches Found![/]\n");

                    var chosenOrder = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("[yellow bold]Sort by Ascending or Descending?[/]")
                        .AddChoices("Ascending", "Descending"));

                    IOrderedEnumerable<Trade> sortedTradesAboveChosenValue;

                    if (chosenOrder == "Ascending")
                    {
                        sortedTradesAboveChosenValue = tradesAboveChosenValue.OrderBy(trade => trade.Price * trade.Quantity).ThenBy(trade => trade.Date);
                    }
                    else
                    {
                        sortedTradesAboveChosenValue = tradesAboveChosenValue.OrderByDescending(trade => trade.Price * trade.Quantity).ThenByDescending(trade => trade.Date);
                    }

                    foreach (var trade in sortedTradesAboveChosenValue)
                    {
                        var value = trade.Price * trade.Quantity;

                        tradesAboveChosenValueTable.AddRow(
                            trade.Id.ToString(),
                            $"[yellow]{trade.Symbol}[/]",
                            trade.Date.ToString("yyyy-MM-dd"),
                            trade.Price.ToString("C"),
                            trade.Quantity.ToString(),
                            trade.Type.ToString(),
                            value.ToString("C")
                            );

                    }

                    AnsiConsole.Write(tradesAboveChosenValueTable);
                }

                else
                {
                    AnsiConsole.MarkupLine("[red bold]There are no trades that meet or exceed that value![/]");
                }

                string enteredStockSymbol = AnsiConsole.Ask<string>("[yellow bold]Enter a stock symbol to search for[/]");

                var stocksWithEnteredSymbol = stocks.SingleOrDefault(stock => stock.Symbol.Equals(enteredStockSymbol, StringComparison.OrdinalIgnoreCase));

                if (stocksWithEnteredSymbol is not null)
                {
                    AnsiConsole.MarkupLine("[green bold]This symbol does exist![/]");
                }
                else AnsiConsole.MarkupLine("[red bold]This symbol does not exist![/]");

                if (ExitReport() == "exit")
                {
                    break;
                }
            }
            AnsiConsole.Clear();
        }

        public static void ReportMinMaxValue(List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();

                decimal minValue = AnsiConsole.Ask<decimal>("[yellow bold]Enter a minimum trade value to filter results:[/] ");
                AnsiConsole.Clear();
                decimal maxValue = AnsiConsole.Ask<decimal>("[yellow bold]Enter a max trade value to filter results:[/] ");
                AnsiConsole.Clear();

                var filteredTrades = trades.Where(trade => (trade.Quantity * trade.Price >= minValue && (trade.Quantity * trade.Price) <= maxValue));

                if (filteredTrades.Any())
                {
                    AnsiConsole.MarkupLine("[green bold]Matches Found![/]");
                    var order = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Ascending", "Descending").Title("[yellow bold]Order by value Ascending, or Descending?[/]"));

                    var sortedTrades = order == "Ascending" ? filteredTrades.OrderBy(trade => trade.Quantity * trade.Price) : filteredTrades.OrderByDescending(trade => trade.Quantity * trade.Price);

                    Table filteredTradesTable = new Table()
                    .Title($"[yellow bold]Trades with values between [bold blue]{minValue:C}[/] and [bold blue]{maxValue:C}[/][/]")
                    .Border(TableBorder.Rounded);

                    filteredTradesTable.AddColumn("[cyan bold]Id[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Symbol[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Date[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Price[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Quantity[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Type[/]");
                    filteredTradesTable.AddColumn("[cyan bold]Value[/]");

                    foreach (var trade in sortedTrades)
                    {
                        var value = trade.Quantity * trade.Price;

                        filteredTradesTable.AddRow(
                            trade.Id.ToString(),
                            $"[yellow]{trade.Symbol}[/]",
                            trade.Date.ToString("yyyy-MM-dd"),
                            trade.Price.ToString("C"),
                            trade.Quantity.ToString(),
                            trade.Type.ToString(),
                            value.ToString("C")
                            );
                    }

                    AnsiConsole.Write(filteredTradesTable);
                }
                else AnsiConsole.MarkupLine($"[red bold]There are no trades between {minValue:C} and {maxValue:C}[/]");

                if (ExitReport() == "exit")
                {
                    break;
                }
            }

        }

        public static void ReportTopFiveValueTrades(List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var topFiveTrades = trades.OrderByDescending(trade => trade.Quantity * trade.Price).Take(5).ToList();

                Table topFiveTradesTable = new Table()
                    .Title("Top 5 trades based on value")
                    .Border(TableBorder.Rounded);

                topFiveTradesTable.AddColumn("[cyan bold]Id[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Symbol[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Date[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Price[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Quantity[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Type[/]");
                topFiveTradesTable.AddColumn("[cyan bold]Value[/]");

                if (topFiveTrades.Any())
                {
                    foreach (var trade in topFiveTrades)
                    {
                        var value = trade.Quantity * trade.Price;

                        topFiveTradesTable.AddRow(
                            trade.Id.ToString(),
                            $"[yellow]{trade.Symbol}[/]",
                             trade.Date.ToString("yyyy-MM-dd"),
                            trade.Price.ToString("C"),
                             trade.Quantity.ToString(),
                             trade.Type.ToString(),
                             value.ToString("C")
                            );
                    }

                    AnsiConsole.Write(topFiveTradesTable);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red bold]There are no trades to print![/]");
                }

                if (ExitReport() == "exit")
                {
                    break;
                }
            }    

        }

        public static void ReportTradesBasedOnDateRange(List<Trade> trades)
        {
            while (true)
            {
                AnsiConsole.Clear();
                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

                    startDate = AnsiConsole.Prompt(new TextPrompt<DateTime>("Enter a start date to filter the data (yyyy-MM-dd): ").DefaultValue(DateTime.Today).ValidationErrorMessage("[red]Invalid date format[/]"));
                    endDate = AnsiConsole.Prompt(new TextPrompt<DateTime>("Enter an end date to filter the data (yyyy-MM-dd): ")
                        .DefaultValue(DateTime.Today)
                        .ValidationErrorMessage("[red]Invalid date format[/]")
                        .Validate(date => date >= startDate
                            ? ValidationResult.Success()
                            : ValidationResult.Error("End date cannot be earlier than the start date!")
                            )
                        );

                var filteredTradesByDateRange = trades.Where(trade => trade.Date.Date >= startDate.Date && trade.Date.Date <= endDate.Date).OrderByDescending(trade => trade.Date).ToList();

                if (filteredTradesByDateRange.Any())
                {
                    Table filteredTradesByDateRangeTable = new Table()
                        .Title($"[yellow bold]Trades between [blue bold]{startDate.ToString("yyyy-MM-dd")}[/] and [blue bold]{endDate.ToString("yyyy-MM-dd")}[/][/]")
                        .Border(TableBorder.Rounded);

                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Id[/]");
                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Symbol[/]");
                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Date[/]");
                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Price[/]");
                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Quantity[/]");
                    filteredTradesByDateRangeTable.AddColumn("[cyan bold]Type[/]");

                    foreach (var trade in filteredTradesByDateRange)
                    {
                        filteredTradesByDateRangeTable.AddRow(
                            trade.Id.ToString(),
                            $"[yellow]{trade.Symbol}[/]",
                            trade.Date.ToString("yyyy-MM-dd"),
                            trade.Price.ToString("C"),
                            trade.Quantity.ToString(),
                            trade.Type.ToString()
                            );
                    }

                    AnsiConsole.Write(filteredTradesByDateRangeTable);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red bold]There are no trades within the specified dates[/]");
                }

                if (ExitReport() == "exit")
                {
                    break;
                }
            }
              
        }

        public static void ReportStocksOrderedOnUserInput(List<Stock> stocks)
        {
            while (true)
            {
                AnsiConsole.Clear();
                string[] orderByVariables = { "Symbol", "Company Name", "Sector", "Trades" };

                string variableToOrderBy = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(orderByVariables).Title("[yellow bold]Select a variable to order by:[/]"));
                AnsiConsole.Clear();
                string ascendingOrDescendingOrder = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Ascending", "Descending").Title("[yellow bold]Order by Ascending or Descending?[/]"));
                AnsiConsole.Clear();


                IOrderedEnumerable<Stock> orderedStocks = null;

                switch (variableToOrderBy)
                {
                    case "Symbol":
                        orderedStocks = ascendingOrDescendingOrder == "Ascending" ? stocks.OrderBy(stock => stock.Symbol) : stocks.OrderByDescending(stock => stock.Symbol);
                        break;
                    case "Company Name":
                        orderedStocks = ascendingOrDescendingOrder == "Ascending" ? stocks.OrderBy(stock => stock.CompanyName) : stocks.OrderByDescending(stock => stock.CompanyName);
                        break;
                    case "Sector":
                        orderedStocks = ascendingOrDescendingOrder == "Ascending" ? stocks.OrderBy(stock => stock.Sector) : stocks.OrderByDescending(stock => stock.Sector);
                        break;
                    case "Trades":
                        orderedStocks = ascendingOrDescendingOrder == "Ascending" ? stocks.OrderBy(stock => stock.Trades.Count) : stocks.OrderByDescending(stock => stock.Trades.Count);
                        break;
                }

                if (orderedStocks != null && orderedStocks.Any())
                {
                    Table orderedStocksTable = new Table()
                        .Title($"[yellow bold]Stocks ordered by [blue bold]{variableToOrderBy}[/] [blue bold]{ascendingOrDescendingOrder}[/].[/]")
                        .Border(TableBorder.Rounded);

                    orderedStocksTable.AddColumn("[cyan bold]Symbol[/]");
                    orderedStocksTable.AddColumn("[cyan bold]Company Name[/]");
                    orderedStocksTable.AddColumn("[cyan bold]Sector[/]");
                    orderedStocksTable.AddColumn("[cyan bold]Trades[/]");


                    foreach (var stock in orderedStocks)
                    {
                        string tradeCount = stock.Trades.Count.ToString();

                        orderedStocksTable.AddRow(
                            $"[yellow bold]{stock.Symbol}[/]",
                            stock.CompanyName,
                            stock.Sector,
                            tradeCount
                            );
                    }

                    AnsiConsole.Write(orderedStocksTable);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red bold]There are no stocks to display[/]");
                }

                if (ExitReport() == "exit")
                {
                    break;
                }
            }
           
        }

        static internal string ExitReport()
        {
            AnsiConsole.WriteLine();
            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("exit", "continue").Title("[bold yellow]Exit?[/]"));

            return userInput;
        }
    } 

    public class PortfolioSummaryRow
    {
        public int TotalStocks { get; set; }
        public long TotalTrades { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalTradeValue { get; set; }
        public decimal AverageTradePrice { get; set; }
        public decimal LowestTradePrice { get; set; }
        public decimal HighestTradePrice { get; set; }
        public DateTime EarliestTradeDate { get; set; }
        public DateTime LatestTradeDate { get; set; }
    }

    public class StockActivityReportRow
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int TradeCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal HighestTradePrice { get; set; }
        public DateTime LatestTradeDate { get; set; }
    }

    public class UniqueValues
    {
        public string Sectors { get; set; } = string.Empty;
        public string StockSymbols { get; set; } = string.Empty;
        public string TradeSymbols { get; set; } = string.Empty;
        public string TradeTypes { get; set; } = string.Empty;
    }
}
