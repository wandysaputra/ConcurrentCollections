// See https://aka.ms/new-console-template for more information

using ConcurrentDictionary;

var line = string.Concat(Enumerable.Repeat("=", 30));
Console.WriteLine(line);
Console.WriteLine("NON-CONCURRENT STOCK");
Console.WriteLine(line);
StockController controller = new StockController(TShirtProvider.AllShirts);
TimeSpan workDay = new TimeSpan(0, 0, 0, 0, 500);

new SalesPerson("Kim").Work(workDay, controller);;

controller.DisplayStock();
Console.WriteLine(line);

Console.WriteLine();
Console.WriteLine(line);
Console.WriteLine("CONCURRENT STOCK");
Console.WriteLine(line);

var concurrentController = new StockConcurrentController(TShirtProvider.AllShirts);
workDay = new TimeSpan(0, 0, 0, 0, 500);

var taskKim = Task.Run(() => new SalesPerson("Kim").Work(workDay, concurrentController));
var taskNick = Task.Run(() => new SalesPerson("Nick").Work(workDay, concurrentController));
var taskRonald = Task.Run(() => new SalesPerson("Ronald").Work(workDay, concurrentController));

Task.WaitAll(taskKim, taskNick, taskRonald);

concurrentController.DisplayStock();

Console.WriteLine(line);
