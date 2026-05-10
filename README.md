Task-Tracker is an ASP.NET Core Razor Pages web application for creating, updating, deleting and managing assignments across projects.

# Prerequisites
- GitHub Codespaces or Visual Studio
- .NET 10 SDK
- GitHub account

# Setup
1. Clone the repository.
2. Open in Codespaces or Visual Studio.
3. From the project root run:
   dotnet ef database update
4. Run the app:
   dotnet run

# Wireframes and the ERD Diagrams:
Use the public viewer URLs listed in the Project Plan document. 

# Features
- Create, read, update, delete assignments
- Filter by status
- Validation with data annotations
- SQLite via EF Core Code-First

# Development Notes
- Models in /Models
- DbContext in /Data/AssignmentsContext.cs
- Razor Pages in /Pages
