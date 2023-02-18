// See https://aka.ms/new-console-template for more information
try
{
    var ordersQueue = new Queue<string>();
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

static void PlaceOrders(Queue<string> orders, string customerName, int nOrders)
{
    for (int i = 1; i <= nOrders; i++)
    {
        Thread.Sleep(1);
        string orderName = $"{customerName} wants t-shirt {i}";
        orders.Enqueue(orderName);
    }
}