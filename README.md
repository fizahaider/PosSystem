# PosSystem WebAPI

Short description
---------------
This is an ASP.NET Core Console & Web API version of a POS-style application originally built as a console app. It demonstrates building RESTful APIs with both in-memory and EF Core–backed persistence and covers common Web API patterns including validation, dependency injection, middleware, and API documentation (Swagger).

Prerequisites
-------------
- .NET SDK (version used by the project)
- SQL Server or LocalDB (if using EF Core SQL provider)
- dotnet-ef CLI (optional, for migrations): `dotnet tool install --global dotnet-ef`

Quick start
-----------
1. Clone the repo:
   `git clone https://github.com/fizahaider/PosSystem`
2. Open the solution in Visual Studio or use the command line
3. Configure the connection string in `appsettings.json` (replace placeholders):
   ```json
   "ConnectionStrings": { "DefaultConnection": "Server=.;Database=PosDb;Trusted_Connection=True;" }
   ```
4. Apply EF migrations (if using SQL Server):
   `dotnet ef database update`
5. Run the API:
   `dotnet run`
6. Open Swagger UI while the app is running: `https://localhost:{PORT}/swagger`

Running with In-Memory DB
-------------------------
- The project supports an in-memory provider for fast local development and testing. Toggle the provider where the DbContext is registered in `Program.cs` or `Startup.cs`.

Database & Migrations
---------------------
- EF Core is used for persistence (DbContext, DbSet).
- Create migrations: `dotnet ef migrations add <Name>`
- Apply migrations: `dotnet ef database update`
- Database seeding is provided (see Data/ seeding or DbContext initializer); it can run on startup or be applied via migrations.

Concepts covered
----------------
- Controllers, routing, REST, HTTP methods, and status codes
- DTOs, model binding, data annotations, and validation
- Dependency injection, services, filters, and middleware
- Swagger / OpenAPI documentation
- EF Core: DbContext, DbSet, connection strings, entity mapping
- Relationships, migrations, seeding, EF CRUD, and LINQ with EF Core

Project layout (high-level)
---------------------------
- Controllers/     — API controllers and route endpoints
- Models/          — Domain entities
- DTOs/            — Request/response DTOs
- Data/            — DbContext, migrations, seeding
- Services/        — Business logic / service layer
- Filters/         — logging filters
- Middleware/      — Custom middleware components
- Migrations/      — EF migration files

Testing done so far
----------------
- Use the in-memory provider for unit/integration tests to avoid a DB dependency.
- Validate models with DataAnnotations and test error responses via Swagger.
