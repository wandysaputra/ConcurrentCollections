# ConcurrentCollections

These might happens when two tasks are concurrently modifying the queue and `Queue<T>` is not `thread-safe`

- Weird result like empty string in this case 
- Throw exception