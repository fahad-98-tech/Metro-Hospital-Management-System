Metro Hospital Management System Technology: ASP.NET (C#) + Bootstrap

System Requirements - Visual Studio (2019 or later recommended) -
Microsoft SQL Server - SQL Server Management Studio (SSMS)

Getting Started

1.  Download & Open Project

-   Download or extract the project folder
-   Open Visual Studio
-   Click File → Open → Project/Solution
-   Select the .sln file from the project folder

Database Setup

Method 1: Using SQL Script (Recommended) - Open SQL Server Management
Studio (SSMS) - Connect to your SQL Server - Open the provided .sql
file - Click Execute This will automatically create the database and all
required tables.

Method 2: Using Database Files (.mdf / .ldf) - Open SQL Server
Management Studio - Right-click Databases → Attach - Click Add - Select
the provided .mdf file - Click OK

Important Step - Right-click the database → Properties - Go to
Permissions - Grant Full Control (Full Access) to all users

Connect Database to Application - Open the project in Visual Studio - Go
to Web.config - Find and update the connection string with your server
and database name

Run the Application - Press F5 or click Start - The project will run in
your browser

