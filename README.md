### How to reproduce

1. Run the `MyService` WCF service with IIS Express.
2. Add a web reference to the `MyClient`: `https://localhost:44364/Service.svc`
3. Run the `MyClient` project. 
4. Observe that the `GetData` method is executed, even though it shouldn't be, because the Policy rejects the request. 