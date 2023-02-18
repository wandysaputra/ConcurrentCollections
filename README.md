# ConcurrentCollections

These might happens when two tasks are concurrently modifying the queue and `Queue<T>` is not `thread-safe`

- Weird result like empty string in this case

![image](https://user-images.githubusercontent.com/42372928/219827540-b93ad708-7f68-41b8-aea8-142a7b0282c2.png)

- Throw exception

![image](https://user-images.githubusercontent.com/42372928/219827556-d3d3b5e8-0a55-4b91-9e2e-14c7bdd3afc5.png)
