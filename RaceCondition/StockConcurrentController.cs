using System.Collections.Concurrent;

namespace RaceCondition;

public class StockConcurrentController
{
    private readonly ConcurrentDictionary<string, int> _stock = new();
    int _totalQuantityBought;
    int _totalQuantitySold;
    public void BuyShirts(string code, int quantityToBuy)
    {
        if (!_stock.ContainsKey(code))
        {
            _stock.TryAdd(code, 0); // allows for failure if other threads have added the item
        }
        // _stock[code] not support multi-thread logic
        // _stock[code] += quantityToBuy;

        // BAD solution
        //int currentStock = _stock[code]; // this will corrupt the data
        //int newStock = currentStock + quantityToBuy;
        //bool success = _stock.TryUpdate(code, newStock, currentStock);         

        // GOOD solution
        // Updates the dictionary, but adds the value if necessary.
        _stock.AddOrUpdate(code, quantityToBuy, (key, oldValue) => oldValue + quantityToBuy);
        // To protect against RACE CONDITIONS requires just only one method call on the collection

        /*
        Whenever there is multiple call on the collection there is a chance other threads can modify the collection between calls
        Examples:
            void SomeFunction(string code, int quantityToBuy){
                _stock.TryAdd(code, quantityToBuy); => method call is thread-safe
                
                // some logic here => Risk of race condition between method calls
                
                _stock.TryGetValue(code, out int stockLevel);  => method call is thread-safe
            }
        */

        /* GOOD PRACTICE GUIDELINE
         * Aim for one single concurrent collection method call per operation
         *
         */

        // _totalQuantityBought += quantityToBuy; // not support multi-thread logic or not thread safe
        Interlocked.Add(ref _totalQuantityBought, quantityToBuy);
        // Interlocked.Add() Thread-safe, atomic equivalent to `+=`
        // Interlocked provides thread-safe versions to some simple operations
    }

    public bool TrySellShirt(string code)
    {
        // ORIGINAL 
        /*if (_stock.TryGetValue(code, out int stock) && stock > 0)
        {
            --_stock[code];
            ++_totalQuantitySold;
            _stock.AddOrUpdate(code, 0, (key, oldValue) => oldValue - 1);
            Interlocked.Increment(ref _totalQuantitySold);
            return true;
        }
        else
            return false;
        */
        /* BAD SOLUTION
         As we leave `_stock.TryGetValue`
         if (_stock.TryGetValue(code, out int stock) && stock > 0)
         {
             _stock.AddOrUpdate(code, 0, (key, oldValue) => oldValue - 1);
             Interlocked.Increment(ref _totalQuantitySold);
             return true;
         }
         *
         */
        // GOOD SOLUTION
        bool success = false;
        
        int newStockLevel = _stock.AddOrUpdate(code, (itemName) =>
        {
            // why need to reset the Closure variable?
            // The lambdas we supplied might be invoked multiple time, as `AddOrUpdate` isn't atomic. So internally has to look out for other threads.
            // Make sure the lambdas can execute more than once without causing bugs
            success = false;
            return 0;
        }, (itemName, oldValue) =>
        {
            if (oldValue == 0)
            {
                success = false;
                return 0;
            }
            else
            {
                success = true;
                return oldValue - 1;
            }
        });

        if (success)
        {
            Interlocked.Increment(ref _totalQuantitySold);
        }

        return success;
    }

    public void DisplayStock()
    {
        Console.WriteLine("Stock levels by item:");
        foreach (TShirt shirt in TShirtProvider.AllShirts)
        {
            // `GetOrAdd()` ensures the item is in the dictionary, `TryGetValue()` doesn't
            // _stock.TryGetValue(shirt.Code, out int stockLevel);
            int stockLevel = _stock.GetOrAdd(shirt.Code, 0);
            Console.WriteLine($"{shirt.Name,-30}: { stockLevel}");
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