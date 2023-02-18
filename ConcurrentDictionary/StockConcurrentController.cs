using System.Collections.Concurrent;

namespace ConcurrentDictionary;

public enum SelectResult {Success, NoStockLeft, ChosenShirtSold }

public class StockConcurrentController
{
    // private Dictionary<string, TShirt?> _stock;
    private ConcurrentDictionary<string, TShirt?> _stock;

    public StockConcurrentController(IEnumerable<TShirt> shirts)
    {
        // _stock = shirts.ToDictionary(x => x.Code);
        _stock = new ConcurrentDictionary<string, TShirt?>(shirts.ToDictionary(x => x.Code));
    }
    public bool Sell(string code)
    {
        // _stock.Remove(code);
        return _stock.TryRemove(code, out var shirtRemoved); // allows for FAILURE
    }
    public (SelectResult Success, TShirt? shirt) SelectRandomShirt()
    {
        var keys = _stock.Keys.ToList();
        if (keys.Count == 0)
        {
            return (SelectResult.NoStockLeft, null);    // all shirts sold
        }

        Thread.Sleep(Rnd.NextInt(10));
        string selectedCode = keys[Rnd.NextInt(keys.Count)];
        
        return _stock.TryGetValue(selectedCode, out TShirt? shirt) 
            ? (SelectResult.Success, shirt) 
            : (SelectResult.ChosenShirtSold, null);
        // return _stock[selectedCode];
    }
    public void DisplayStock()
    {
        Console.WriteLine($"\r\n{_stock.Count} items left in stock:");
        foreach (TShirt? shirt in _stock.Values)
            Console.WriteLine(shirt);
    }
}