# Welcome to Librarian

This solution consists of an API and 2 Web apps. 

- Vue.js app
- ASP.net MVC app

Please make sure you run the API application first before starting up the web applications. 

In the SupportWave.Librarian > SupportWave.Librarian.Web there are 2 folders:

- MVC
- Vue

To run MVC app, either build with CLI and run with cli:

dotnet build 
dotnet test
dotnet run

or run with visual studio/ visual studio code or rider. 

For the Vue.js app there is a readme provided in the app folder with instructions to run.

Please note: Unit tests are only the happy path as I have run out of time, the proper implementation would have tested validation as well as failure cases to get the highest code coverage. A set of full integration test could be implemented as well to make sure that the code writes to json file as expected and has multi-threaded support.

## Happy code reviewing :) 