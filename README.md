# [ConcurrentCollections](https://github.com/wandysaputra/ConcurrentCollections/tree/master/ConcurrentCollections)

These might happens when two tasks are concurrently modifying the queue and `Queue<T>` is not `thread-safe` 
https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-7.0#:~:text=Any%20instance%20members%20are%20not%20guaranteed%20to%20be%20thread%20safe.


- Weird result/data corruption, like empty string in this case

![image](https://user-images.githubusercontent.com/42372928/219827540-b93ad708-7f68-41b8-aea8-142a7b0282c2.png)

- Throw exception

![image](https://user-images.githubusercontent.com/42372928/219827556-d3d3b5e8-0a55-4b91-9e2e-14c7bdd3afc5.png)


# [Concurrent Dictionary](https://github.com/wandysaputra/ConcurrentCollections/tree/master/ConcurrentDictionary)
## Conversion Dictionary to ConcurrentDictionary

StockController vs StockConcurrentController

- Favor `TryXXX()` style methods, which don't presume knowledge of the state


# [Race Condition](https://github.com/wandysaputra/ConcurrentCollections/tree/master/RaceCondition)

- To protect against RACE CONDITIONS requires just only one method call on the collection
- Whenever there is multiple call on the collection there is a chance other threads can modify the collection between calls
  
  Examples:
    ```
    void SomeFunction(string code, int quantityToBuy){
        _stock.TryAdd(code, quantityToBuy); // => method call is thread-safe

        // some logic here => Risk of race condition between method calls

        _stock.TryGetValue(code, out int stockLevel); // => method call is thread-safe
    }

    ```
