# Vehicle-Manager-Platform
This is an ASP.NET MVC Web App for Vehicle management

Information will be added on 10.08.2023

# Overview:
Vehicle-Manager-Platform (VMP) is a web application for managing waybills of vehicles and tracking upcoming maintenance checks.
It supports the automatic calculation of fuel costs for a certain vehicle and setting notifications for upcoming periodic costs.

# The Technologies used in the web application:
 - ASP.NET Core (MVC pattern)
 - .NET Core 6.0
 - Entity Framework Core
 - HTML
 - CSS
 - Bootstrap 5
 - MS SQL Server
 - NUnit
 - Moq
 - IromXL (for generating Excel files)

# Roles in this application:
- This application supports 2 types of accounts: **User** and **Admin**.
- The application comes with a default admin account
  - **Admin Username** - admin@admin.bg
  - **Admin password** - Admin123
- Every new account created will automatically get the **User** role and the admin account can assign the **Admin** role
from the **Admin panel** of the application.

# Diagram of the Database can be found here:
Link: ![Database Diagram]([https://github.com/logos/github-logo.png](https://github.com/kvinceto/Vehicle-Manager-Platform/blob/main/Diagram.jpg)https://github.com/kvinceto/Vehicle-Manager-Platform/blob/main/Diagram.jpg)

# Instruction on how to start the application:
1.	Clone the VMP repository from GitHub.
2.	Open the solution file (Vehicle-Manager-Platform.sln) in Visual Studio.
3.	Build the solution to restore NuGet packages.
4.	Configure the database connection string in appsettings.json.
5.	Go to Package Manager Console and Update Database. This will seed the Admin account also.
6.	Run the application. This will create the Roles in the Database.
7.	You are ready to use the application. If you want some example data there is a SQL Query file that can be used to populate your database.


# Functionality:
## Tasks:
Every user can create Tasks to manage his work with the app. For example:  A new task to remind him to check if a certain vehicle has reached the required mileage for maintenance. Or a reminder to enter all the waybills for this month not later than a certain date.  

There are 3 options from the Task Dropdown menu:
- **All Tasks** - This will show you all the active tasks in the system, for every User.
- **My Tasks** - This will show you only your active tasks and will give you the option to edit them or delete them.
- **Add Task** - This will allow you to create a new Task.

## Owners:
Every vehicle in the system must be assigned to an Owner.  

There are 2 options from the Owner Dropdown menu:
- **All Owners** - This will show you all the active Owners in the system and allow you to edit or delete them.  
- **Add Owner** - This will allow you to create a new Owner.

## Cost center:
Every waybill in the system must be assigned to a Cost center.  

There are 2 options from the Cost center Dropdown menu:
- **All Cost centers** - This will show you all the active Cost centers in the system and allow you to edit or delete them.  
- **Add New Cost center** - This will allow you to create a new Cost center.

## Vehicles:
This is the section that manages the vehicles entered into the system.  

There are 2 options from the Vehicles Dropdown menu:
- **All Vehicles** - This will show you all the active Vehicles in the system and allow you to edit or delete them.  
- **Add New Vehicle** - This will allow you to create a new Vehicle.

## Checks:
This section will give you access to two types of checks that this app supports. A Mileage check and a Date check.  
A Mileage check is a reminder set to a specific mileage that has to be reached for an event to occur. Like the Oil change or a warranty expiration of a new vehicle.  
A Date check is a reminder set to a specific date for an event to occur. Like insurance expiration.  

There are 4 options from the Checks Dropdown menu:
- **All MileageChecks** - This will show you all the active Mileage checks in the system and allow you to edit or delete them.  
- **All DateChecks** - This will show you all the active Date checks in the system and allow you to edit or delete them.
- **Add New MileageCheck** - This will allow you to create a new Mileage check for a vehicle.
- **Add New DateeCheck** - This will allow you to create a new Date check for a vehicle.

## Waybills:
This section will give you access to the waybills in the system.

There are 2 options from the Waybills Dropdown menu:
- **All Waybills** - This will show you all the waybills for a certain vehicle for the chosen period.  
- **Add New Waybill** - This will allow you to create a new Waybill for a vehicle.


## Admin panel:
This section will give you access to the admin functionalities in the system.

There are two types of operations **Entity restorations** and **User management**.  
The admin can restore deleted tasks, owners, vehicles, cost centers, and checks. It can also change the roles of other users and also delete them.
