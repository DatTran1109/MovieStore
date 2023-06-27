# MovieStore
MovieStore is a simple Asp .Net Core MVC web application. Normal user can register an account, login to the web and view all available movies. Admin can manage movies and movie genres.

**Guide to run project:**
First of all, be sure that you have installed Microsoft Visual Studio and SQL Server on your computer.

1. Download project and open it with Visual Studio.
2. Go to appsettings.json file then look for ConnectionStrings line (on line 9). Edit "data source = YourSqlServerName" (you can open Sql Server Management Studio to check YourSqlServerName).
3. Open Package Manager Console by go to Tools -> NuGet Package Manager -> Package Manager Console.
4. Type update-database then press enter.
5. Run project by click run button on Visual Studio.

**For admin functionalities, type /UserAuthentication/CreateAdmin on URL bar after your localhost to create a default admin account (username is Admin and password is Admin@123).**
