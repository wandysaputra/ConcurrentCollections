// See https://aka.ms/new-console-template for more information

using RaceCondition;

var line = string.Concat(Enumerable.Repeat("=", 30));

Console.WriteLine("RACE CONDITION");

Console.WriteLine(line);
Console.WriteLine("NON-CONCURRENT");
Console.WriteLine(line);

var controller = new StockController();
var workDay = new TimeSpan(0, 0, 0, 0, 500);

new SalesPerson("Tim").Work(workDay, controller);

controller.DisplayStock();

Console.WriteLine(line);
Console.WriteLine();

Console.WriteLine(line);
Console.WriteLine("CONCURRENT");
Console.WriteLine(line);

var concurrentController = new StockConcurrentController();
workDay = new TimeSpan(0, 0, 0, 0, 500);

var taskTim = Task.Run(() => new SalesPerson("Tim").Work(workDay, concurrentController));
var taskAgnes = Task.Run(() => new SalesPerson("Agnes").Work(workDay, concurrentController));
var taskSophie = Task.Run(() => new SalesPerson("Sophie").Work(workDay, concurrentController));
var taskMark = Task.Run(() => new SalesPerson("Mark").Work(workDay, concurrentController));

Task.WaitAll(taskAgnes, taskTim, taskMark, taskSophie);

controller.DisplayStock();
Console.WriteLine(line);