# ASP.NET Core Service Template

ASP.NET Core 3.1 template for creating a basic windows service w/logging, httpclient, and a background hosted service.

## Importing the template

Visual Studio provides an Export Template Wizard that can be used to update an existing template:

1. Open the Service template project in Visual Studio.
2. On the Project menu, choose Export Template.
3. Follow the prompts in the wizard to export the template as a .zip file.
4. Restart Visual Studio, and then use the search bar in the New Project dialog to help you find the project template you just added.

## Whats included?

* Sentry error logging ~ [https://sentry.io/](https://sentry.io/)
* Serilog logging framework ~ [https://serilog.net/](https://serilog.net/)
* Windows service extensions ~ [https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.1&tabs=visual-studio](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.1&tabs=visual-studio)
* Http Client for your api ~ [https://api.clientvantage.ca](https://api.clientvantage.ca)
* Connection to an OPENID auth server ~ [https://is4.clientvantage.ca](https://is4.clientvantage.ca)
* Background worker task already setup ~ [https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice)
