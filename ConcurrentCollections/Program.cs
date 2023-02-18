// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

try
{
    var ordersQueue = new ConcurrentQueue<string>();
    var task1 = Task.Run(() => PlaceOrders(ordersQueue, "Xavier", 5));
    var task2 = Task.Run(() => PlaceOrders(ordersQueue, "Ramdevi", 5));

    Task.WaitAll(task1, task2);

    foreach (string order in ordersQueue)
    {
        Console.WriteLine("ORDER: " + order);
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    Console.ReadKey();
}

static void PlaceOrders(ConcurrentQueue<string> orders, string customerName, int nOrders)
{
    for (int i = 1; i <= nOrders; i++)
    {
        Thread.Sleep(new TimeSpan(1));
        string orderName = $"{customerName} wants t-shirt {i}";
        orders.Enqueue(orderName);
    }
}