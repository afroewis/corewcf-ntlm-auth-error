### How to reproduce

Steps 1-2 shouldn't be necessary, as the MyService client is already in the repository. 

1. Run the `MyService` WCF service with IIS Express.
2. Add a web reference to the `MyClient`: `https://localhost:44364/Service.svc`
3. Create a local Windows user with the username `wcftest` and password `wcftest`
4. Run the `MyClient` project. 
5. Observe that the `GetData` method is executed, even though it shouldn't be, because the Policy rejects the request. 