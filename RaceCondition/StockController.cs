namespace RaceCondition;

public class StockController
{
    private readonly Dictionary<string, int> _stock = new();
    int _totalQuantityBought;
    int _totalQuantitySold;
    public void BuyShirts(string code, int quantityToBuy)
    {
        if (!_stock.ContainsKey(code))
            _stock.Add(code, 0);
        _stock[code] += quantityToBuy;
        _totalQuantityBought += quantityToBuy;
    }

    public bool TrySellShirt(string code)
    {
        if (_stock.TryGetValue(code, out int stock) && stock > 0)
        {
            --_stock[code];
            ++_totalQuantitySold;
            return true;
        }
        else
            return false;
    }

    public void DisplayStock()
    {
        Console.WriteLine("Stock levels by item:");
        foreach (TShirt shirt in TShirtProvider.AllShirts)
        {
            _stock.TryGetValue(shirt.Code, out int stockLevel);
            Console.WriteLine($"{shirt.Name,-30}: {stockLevel}");
        }

        // audit buys, sells, and stocks
        int totalStock = _stock.Values.Sum();
        Console.WriteLine($"\r\nBought = {_totalQuantityBought}");
        Console.WriteLine($"Sold   = {_totalQuantitySold}");
        Console.WriteLine($"Stock  = {totalStock}");
        int error = totalStock + _totalQuantitySold - _totalQuantityBought;
        
        Console.WriteLine(error == 0 ? "Stock levels match" : $"Error in stock level: {error}");
    }
}