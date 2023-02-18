namespace ConcurrentDictionary;

public class SalesPerson
{
    public string Name { get; }
    public SalesPerson(string name)
    {
        this.Name = name;
    }
    public void Work(TimeSpan workDay, StockController controller)
    {
        DateTime start = DateTime.Now;
        while (DateTime.Now - start < workDay)
        {
            var (shirtsInStock, status) = ServeCustomer(controller);
            if (status != null)
                Console.WriteLine($"{Name}: {status}");

            if (!shirtsInStock)
                break;
        }
    }
    public (bool ShirtsInStock, string Status) ServeCustomer(
        StockController controller)
    {
        TShirt shirt = controller.SelectRandomShirt();
        if (shirt == null)
            return (false, "All shirts sold");

        Thread.Sleep(Rnd.NextInt(30));

        // customer chooses to buy with only 20% probability
        if (!Rnd.TrueWithProb(0.2))
        {
            return (true, null);
        }

        controller.Sell(shirt.Code);
        return (true, $"Sold {shirt.Name}");
    }

    public void Work(TimeSpan workDay, StockConcurrentController controller)
    {
        DateTime start = DateTime.Now;
        while (DateTime.Now - start < workDay)
        {
            var (shirtsInStock, status) = ServeCustomer(controller);
            if (status != null)
                Console.WriteLine($"{Name}: {status}");
            if (!shirtsInStock)
                break;
        }
    }
    public (bool ShirtsInStock, string? Status) ServeCustomer(
        StockConcurrentController controller)
    {
        var (result, shirt) = controller.SelectRandomShirt();

        switch (result)
        {
            case SelectResult.NoStockLeft:
                return (false, "All shirts sold");
            case SelectResult.ChosenShirtSold:
                return (false, "Can't show shirt to customer - already sold");
        }

        Thread.Sleep(Rnd.NextInt(30));

        // customer chooses to buy with only 20% probability
        if (!Rnd.TrueWithProb(0.2))
        {
            return (true, null);
        }

        return controller.Sell(shirt?.Code ?? string.Empty)
            ? (true, $"Sold {shirt.Name}")
            : (true, $"Can't sell {shirt.Name}: Already sold");
    }

}