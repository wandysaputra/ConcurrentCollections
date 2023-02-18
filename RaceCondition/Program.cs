// See https://aka.ms/new-console-template for more information

using RaceCondition;

Console.WriteLine("RACE CONDITION");

var controller = new StockController();
var workDay = new TimeSpan(0, 0, 0, 0, 500);

new SalesPerson("Tim").Work(workDay, controller);

controller.DisplayStock();