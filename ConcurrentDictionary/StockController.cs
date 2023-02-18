﻿using System.Collections.Concurrent;

namespace ConcurrentDictionary;

public class StockController
{
    private ConcurrentDictionary<string, TShirt> _stock;

    public StockController(IEnumerable<TShirt> shirts)
    {
        _stock = new ConcurrentDictionary<string, TShirt>(shirts.ToDictionary(x => x.Code));
    }
    public void Sell(string code)
    {
        _stock.TryRemove(code, out _);
    }
    public TShirt SelectRandomShirt()
    {
        var keys = _stock.Keys.ToList();
        if (keys.Count == 0)
            return null;    // all shirts sold

        Thread.Sleep(Rnd.NextInt(10));
        string selectedCode = keys[Rnd.NextInt(keys.Count)];
        return _stock[selectedCode];
    }
    public void DisplayStock()
    {
        Console.WriteLine($"\r\n{_stock.Count} items left in stock:");
        foreach (TShirt shirt in _stock.Values)
            Console.WriteLine(shirt);
    }
}