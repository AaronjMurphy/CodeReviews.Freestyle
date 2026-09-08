using Spectre.Console;
using TCSA.Study.Linq;
using TCSA.Study.Linq.Printing;
using TCSA.Study.Linq.Seeders;
Console.OutputEncoding = System.Text.Encoding.UTF8;
//CultureInfo culture = new CultureInfo("en-IE");

List<Stock> stocks = StockSeeder.GetStocks();
List<Trade> trades = stocks
    .SelectMany(stock => stock.Trades)
    .ToList();

// SANDBOX - Write your code here


//
const string stockOption = "Print stocks";
const string tradeOption = "Print all trades";
const string printStockReport = "Print stock report";
const string exitOption = "Exit";

List<string> reportsRun = new();


while (true)
{
    string selectedOption = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold yellow]What would you like to view?[/]")
            .AddChoices(stockOption, tradeOption, printStockReport, exitOption));

    AnsiConsole.Clear();

    if (selectedOption == exitOption)
    {
        break;
    }

    if (selectedOption == stockOption)
    {
        TablePrinter.PrintStocks(stocks);
    }
    else if (selectedOption == tradeOption)
    {
        TablePrinter.PrintTrades(trades);
    }
    else if (selectedOption == printStockReport)
    {
        const string portfolioSummary = "Portfolio Summary";
        const string sectorReport = "Sector Report";
        const string tradesReport = "Trades Report";
        const string hiVolTrades = "High Volume Trades";
        const string stockActivityReport = "Stock Activity Report";
        const string uniqueValuesReport = "Unique Values Report";
        const string dataQualityReport = "Data Quality Report";
        const string tradesValueFilter = "Trades Value Filter (min-max)";
        const string topFiveValueTradesReport = "Top 5 Value Trades Report";
        const string reportTradeByDateRange = "Report Trades By Date Range";
        const string reportStocksOrderedByUser = "Report Stocks Ordered By User Input";

        var reportSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[yellow bold]Choose a report to print.[/]")
            .AddChoices(portfolioSummary, sectorReport, tradesReport, hiVolTrades, stockActivityReport, uniqueValuesReport, dataQualityReport, tradesValueFilter, topFiveValueTradesReport, reportTradeByDateRange, reportStocksOrderedByUser)
        );

        switch (reportSelection)
        {
            case portfolioSummary:
                ReportPrinter.PrintPortfolioSummary(stocks);
                reportsRun.Add(portfolioSummary);
                break;
            case sectorReport:
                ReportPrinter.reportSectors(stocks);
                reportsRun.Add(sectorReport);
                break;
            case tradesReport:
                ReportPrinter.reportTradesBySymbol(stocks, trades);
                reportsRun.Add(tradesReport);
                break;
            case hiVolTrades:
                ReportPrinter.reportHighVolumeTrades(stocks, trades);
                reportsRun.Add(hiVolTrades);
                break;
            case stockActivityReport:
                ReportPrinter.reportStockActivity(stocks);
                reportsRun.Add(stockActivityReport);
                break;
            case uniqueValuesReport:
                ReportPrinter.reportUniqueValues(stocks);
                reportsRun.Add(uniqueValuesReport);
                break;
            case dataQualityReport:
                ReportPrinter.reportDataQuality(stocks, trades);
                reportsRun.Add(dataQualityReport);
                break;
            case tradesValueFilter:
                ReportPrinter.ReportMinMaxValue(trades);
                reportsRun.Add(tradesValueFilter);
                break;
            case topFiveValueTradesReport:
                ReportPrinter.ReportTopFiveValueTrades(trades);
                reportsRun.Add(topFiveValueTradesReport);
                break;
            case reportTradeByDateRange:
                ReportPrinter.ReportTradesBasedOnDateRange(trades);
                reportsRun.Add(reportTradeByDateRange);
                break;
            case reportStocksOrderedByUser:
                ReportPrinter.ReportStocksOrderedOnUserInput(stocks);
                reportsRun.Add(reportStocksOrderedByUser);
                break;
        }
    }

    AnsiConsole.WriteLine();

}

string reportsPanelText = string.Join("\n", reportsRun);

Panel summaryScreen = new Panel(reportsPanelText)
    .Header("[yellow bold]Reports Run[/]")
    .Border(BoxBorder.Rounded);

if (summaryScreen != null)
{
    AnsiConsole.Write(summaryScreen);
}

AnsiConsole.MarkupLine("[yellow bold]Press Enter to exit.[/]");


