// See https://aka.ms/new-console-template for more information

using ConcurrentDictionary;

StockController controller = new StockController(TShirtProvider.AllShirts);
TimeSpan workDay = new TimeSpan(0, 0, 0, 0, 500);

new SalesPerson("Kim").Work(workDay, controller);

controller.DisplayStock();