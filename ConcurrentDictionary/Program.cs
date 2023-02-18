// See https://aka.ms/new-console-template for more information

using ConcurrentDictionary;

StockController controller = new StockController(TShirtProvider.AllShirts);
TimeSpan workDay = new TimeSpan(0, 0, 0, 0, 500);

var taskKim = Task.Run(() => new SalesPerson("Kim").Work(workDay, controller));
var taskNick = Task.Run(() => new SalesPerson("Nick").Work(workDay, controller));
var taskRonald = Task.Run(() => new SalesPerson("Ronald").Work(workDay, controller));

Task.WaitAll(taskKim, taskNick, taskRonald);

controller.DisplayStock();