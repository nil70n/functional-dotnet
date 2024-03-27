# Contoso University 
This demo application is an amalgamation of smaller demo applications found in tutorials at [AspNetCore docs](https://docs.microsoft.com/en-us/aspnet/core/). 

This solution has been modified to suit the needs of the Functional Programming Deep Dive with C# course.


### Requirements
- [Visual Studio 2019 Community](https://visualstudio.microsoft.com/)
- [.NET SDK 2.2.207](https://dotnet.microsoft.com/download/dotnet-core/2.2)


### ContosoUniversity.Web
- Traditional Web App using MVC + Razor Pages
- [Demo](http://contoso-university-web.adrianlimon.com)
### ContosoUniversity.Api
- Traditional Rest Api
- [Demo](http://contoso-university-api.adrianlimon.com/)
- Generate JWT Token at http://contoso-university-web.adrianlimon.com/api/token to access secure api content.  Requires registering via Web App.
### Testing
- Unit Testing using [Moq](https://github.com/Moq/moq4/wiki/Quickstart) and [xUnit](https://xunit.github.io/docs/getting-started-dotnet-core)
- Integration Testing using TestHost and InMemoryDatabase
- UI Testing using Selenium
### Security
- using Identity 2.0
- Confirm Email using [SendGrid](sendgrid.com)
- Confirm Phone using [Twilio](https://www.twilio.com/sms/api)
- Two-Factor Authentication - [see tutorial](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/2fa)
- OAuth 2 - Enable Google & Facebook logins
- JWT (Json Web Token) - use to access secure API
### Technologies
- [ASP.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- Asp.Net Core Mvc 2.2 / Razor 2.2
- Entity Framework Core 2.2 / Identity 2.2
- Moq
- xUnit
- Twilio
- SendGrid

### Design Patterns
- [Repository](https://social.technet.microsoft.com/wiki/contents/articles/36287.repository-pattern-in-asp-net-core.aspx)
- [Unit Of Work](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/advanced)
