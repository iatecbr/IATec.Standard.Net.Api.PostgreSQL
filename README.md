# 🚀 {API_NAME}

> Robust .NET API development template at IATec, promoting standard practices, efficiency and security. Ideal for scalable, high-performance APIs.

---

## 📋 Index

- [About the Project](#about-the-project)
- [Technologies and Stack](#technologies-and-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
- [Configuration](#configuration)
- [Database (PostgreSQL)](#database-postgresql)
- [Health Checks](#health-checks)
- [API Versioning](#api-versioning)
- [API Documentation (Scalar)](#api-documentation-scalar)
- [CORS](#cors)
- [Authentication](#authentication)
- [Tests](#tests)
- [Renaming the API](#renaming-the-api)
- [Docker](#docker)
- [Contributing](#contributing)

---

## About the Project

This repository is a **base template** for creating new .NET APIs following IATec standards. It comes pre-configured with:

- Decoupled layered architecture (Domain, Application, Persistence, AntiCorruption, MessageQueue, CrossCutting, Api).
- **PostgreSQL** database access with **Entity Framework Core**.
- Separate **Read** and **Write** `DbContext` for CQRS-like separation.
- **Snake_case** naming convention for PostgreSQL database objects.
- Automatic application of **EF Core migrations** (non-`Local` environments only).
- API versioning.
- Automatic documentation via **Scalar/OpenAPI**.
- Health Checks with JSON response.
- CORS configuration.
- Integration with shared libraries (`IATec.Shared.*`).
- Validation and fluent results (`FluentValidation`, `FluentResults`).
- **MediatR** for inter-layer communication.

> **Note:** Whenever creating a new API from this template, read the [Renaming the API](#renaming-the-api) section to adjust names and references.

---

## Technologies and Stack

| Technology | Version | Package |
|------------|--------|---------|
| .NET | 10.0 | - |
| ASP.NET Core | 10.0.x | - |
| Scalar.AspNetCore | 2.14.14 | `Scalar.AspNetCore` |
| Microsoft.AspNetCore.OpenApi | 10.0.8 | `Microsoft.AspNetCore.OpenApi` |
| API Versioning (Asp.Versioning.Mvc) | 10.0.0 | `Asp.Versioning.Mvc` |
| API Versioning Explorer | 10.0.0 | `Asp.Versioning.Mvc.ApiExplorer` |
| HealthChecks UI Client | 9.0.0 | `AspNetCore.HealthChecks.UI.Client` |
| MediatR | 14.1.0 | `MediatR` |
| FluentValidation | 12.1.1 | `FluentValidation`, `FluentValidation.DependencyInjectionExtensions` |
| FluentResults | 4.0.0 | `FluentResults` |
| EF Core | 10.0.8 | `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Relational` |
| EF Core Tools | 10.0.8 | `Microsoft.EntityFrameworkCore.Tools` |
| Npgsql EF Core | 10.0.1 | `Npgsql.EntityFrameworkCore.PostgreSQL` |
| EFCore Naming Conventions | 10.0.1 | `EFCore.NamingConventions` |
| IATec.Shared.Api | 1.2.0 | `IATec.Shared.Api` |
| IATec.Shared.Application | 2.0.0 | `IATec.Shared.Application` |
| IATec.Shared.Domain | 2.0.1 | `IATec.Shared.Domain` |
| IATec.Shared.Behaviors | 1.3.0 | `IATec.Shared.Behaviors` |
| IATec.Shared.HttpClient | 3.0.0 | `IATec.Shared.HttpClient` |
| Microsoft.Extensions.* | 10.0.8 | `Microsoft.Extensions.DependencyInjection.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Configuration.Binder`, `Microsoft.Extensions.Options` |

---

## Architecture

The project follows a layered organization inside the `src/` folder:

```
src/
├── Api/                    → ASP.NET Core entrypoint (Controllers, Startup, Configs)
│   ├── Configurations/
│   │   ├── ApiDependencyInjectionConfig.cs
│   │   └── Extensions/
│   │       ├── CorsPolicyExtension.cs
│   │       ├── HealthCheckExtension.cs
│   │       ├── MigrationExtensions.cs
│   │       ├── OptionsExtension.cs
│   │       ├── ScalarConfiguration.cs
│   │       └── VersioningExtension.cs
│   └── Properties/
│       └── launchSettings.json
├── Application/            → Use cases, handlers, application logic
│   ├── Configurations/
│   ├── Dispatchers/
│   └── Features/
├── CrossCutting/           → Shared behaviors, MediatR pipelines
├── Domain/                 → Entities, contracts, validations, pure business rules
│   └── Models/
├── Persistence/            → Data access, EF Core, PostgreSQL, repositories
│   ├── Configurations/
│   │   ├── Options/
│   │   │   ├── PostgreSqlOption.cs
│   │   │   └── EntityFrameworkOption.cs
│   │   └── Extensions/
│   │       └── DatabaseExtension.cs
│   ├── Context/
│   │   ├── ReadDataContext.cs
│   │   └── WriteDataContext.cs
│   └── MIgrations/
├── AntiCorruption/         → Adapters for external services (HTTP Clients)
│   └── Services/
├── MessageQueue/           → Message producers/consumers (ready for expansion)
├── Domain.Tests/           → Domain layer unit tests (framework TBD)
└── Application.Tests/      → Application layer unit tests (framework TBD)
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or compatible higher version)
- [PostgreSQL](https://www.postgresql.org/download/) 14+ (or Docker instance)
- (Optional) Docker for building/publishing images
- Editor of your choice (VS, VS Code, Rider)

---

## How to Run

### 1. Clone the repository

```bash
git clone <repository-url>
cd {API_NAME}
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Configure the database

Ensure PostgreSQL is running and create the database (`dbPeople_local` by default). Update `ServerReader`, `ServerWriter`, and credentials in `src/Api/appsettings.json` if needed.

### 4. Run the API

```bash
dotnet run --project src/Api/Api.csproj
```

By default, the application will be available at:
- `http://localhost:5015`
- Scalar UI: `http://localhost:5015/documentation`

> The active launch profile (`src/Api/Properties/launchSettings.json`) sets the environment to `Local` and opens `/documentation` in the browser.

---

## Configuration

Settings are located in `src/Api/appsettings.json` (and its environment overrides, such as `appsettings.Development.json`).

**Current structure example:**

```json
{
  "TimeZone": "UTC",
  "Container": {
    "Name": "Vertical-ContextContainerType",
    "ContainerId": "ContainerId"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "PostgreSQL": {
    "Database": "dbPeople_local",
    "User": "postgres",
    "Password": "",
    "ServerReader": "localhost",
    "ServerWriter": "localhost",
    "Port": "5432"
  },
  "EntityFramework": {
    "SensitiveDataLogging": true
  }
}
```

### What to configure when starting a new API

| Section | Description | Example |
|---------|-----------|---------|
| `TimeZone` | Application time zone | `"America/Sao_Paulo"` |
| `Container` | Deployment/container metadata | Adjust `Name` and `ContainerId` according to your environment |
| `Logging` | ASP.NET Core log level | `"Debug"`, `"Information"`, `"Warning"` |
| `PostgreSQL` | PostgreSQL connection settings | Change `Database`, `User`, `Password`, `ServerReader`, `ServerWriter`, `Port` |
| `EntityFramework` | EF Core behavior | `SensitiveDataLogging: false` in Production |

> **Tip:** Add new configuration sections in `src/Api/Configurations/Extensions/OptionsExtension.cs` for typed injection via `IOptions<T>`. Currently configured options:
> - `LogServiceOption` → `LogService` section (from `IATec.Shared.Domain.Options`)
> - `ContainerOption` → `Container` section (from `IATec.Shared.Domain.Options`)

---

## Database (PostgreSQL)

This template uses **Npgsql.EntityFrameworkCore.PostgreSQL** with **EFCore.NamingConventions** (snake_case).

### Read / Write Separation

Two `DbContext` instances are registered:

| Context | Purpose | Tracking |
|---------|---------|----------|
| `ReadDataContext` | Queries and reads | `NoTrackingWithIdentityResolution` |
| `WriteDataContext` | Commands and writes | Default tracking enabled |

Both contexts use **snake_case** naming conventions and **sensitive data logging** based on `EntityFramework` settings.

### Migrations

Migrations are automatically applied when the environment is **not** `Local`:

```csharp
// src/Api/Configurations/Extensions/MigrationExtensions.cs
if (app.Environment.EnvironmentName is "Local") return app;

using var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<WriteDataContext>();
dataContext.Database.Migrate();
```

> **Tip:** During development (`Local`), apply migrations manually via CLI:
>
> ```bash
> dotnet ef database update --project src/Persistence --startup-project src/Api
> ```

### Connection String Format

The `PostgreSqlOption` class in the Persistence layer builds the connection string automatically:

```csharp
$"Host={server};Port={Port};Pooling=true;Database={Database};User Id={User};Password={Password}"
```

- `ServerReader` is used for `ReadDataContext`
- `ServerWriter` is used for `WriteDataContext`

---

## Health Checks

The project exposes a health check endpoint that returns the API assembly version:

```
GET /_healthcheck/status
```

Features:
- Returns `Healthy`/`Unhealthy` status.
- Includes the API assembly version (from `Api.csproj <Version>2.0.0</Version>`).
- Response in `HealthChecks.UI.Client` visual format.
- Environment name included as a health check tag.

---

## API Versioning

The project uses **Asp.Versioning.Mvc** with the following configuration:

```csharp
services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    option.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
```

- Default version: **v1.0**
- Version reader: **Query string** (`?api-version=1.0`)
- API versions are reported in response headers

---

## API Documentation (Scalar)

Interactive documentation powered by **Scalar** and native ASP.NET Core **OpenAPI** is available in **non-Production** environments:

- OpenAPI JSON: `/openapi/v1.json`
- Scalar UI: `/documentation`

### Configured features

- Automatically generated from native `Microsoft.AspNetCore.OpenApi`.
- **Mars theme** with forced dark mode.
- Tags expanded and sorted alphabetically.
- Default HTTP client configured as **C# HttpClient**.
- Title includes environment name (e.g. `{API_NAME} - Local`).

---

## CORS

The project comes with a pre-configured CORS policy:

```csharp
services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();

    builder.WithExposedHeaders([
        "X-Custom-Header",
        "Location",
        "Content-Disposition",
        "Content-Length"
    ]);
}));
```

- Policy name: `CorsPolicy`
- Allows **any origin**, **any header**, and **any method**
- Exposes custom headers for HTTP responses

---

## Authentication

Scalar UI already supports sending JWT tokens in the `Authorization` header with the `Bearer` scheme.

To enable actual token validation in the API:
1. Add the desired authentication package (e.g. `Microsoft.AspNetCore.Authentication.JwtBearer`).
2. Configure token validation in `ApiDependencyInjectionConfig.cs` or in a new extension method.
3. Insert `app.UseAuthentication()` before `app.UseAuthorization()` in `UseApi()`.

---

## Tests

The template includes two test projects:

| Project | Layer Tested | Status |
|---------|--------------|--------|
| `Domain.Tests` | Domain | Project exists — add a test framework (xUnit, NUnit, or MSTest) as needed |
| `Application.Tests` | Application | Project exists — add a test framework (xUnit, NUnit, or MSTest) as needed |

> **Note:** The test projects currently have **no test framework installed**. Add your preferred package (e.g. `xunit`, `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio`) before writing tests.

To run all tests:

```bash
dotnet test
```

---

## Renaming the API

> Whenever using this project as a base for a new API, follow the steps below to adjust names and references. The text `{API_NAME}` used throughout this README acts as a placeholder for the **actual project name** you want to use.

### Step-by-step guide

#### 1. Clone the repository and enter the folder

```bash
git clone <repository-url>
cd {API_NAME}
```

#### 2. Rename the Solution file

```bash
mv IATec.Standard.Net.Api.sln {API_NAME}.sln
```

#### 3. Rename `AssemblyName` and `RootNamespace` in `.csproj` files

Open `src/Api/Api.csproj` and other `.csproj` files and add or change:

```xml
<PropertyGroup>
    <AssemblyName>{API_NAME}</AssemblyName>
    <RootNamespace>{API_NAME}</RootNamespace>
</PropertyGroup>
```

> By default these fields are optional and inherit the file name. Explicitly defining them prevents assembly name mismatches after renaming.

#### 4. Adjust C# namespaces

Run a **Replace All** in the `src/` folder for each project layer. Example:

| From | To (example) |
|------|--------------|
| `namespace Api;` | `namespace ProjectName.Api;` |
| `namespace Application;` | `namespace ProjectName.Application;` |
| `namespace Domain;` | `namespace ProjectName.Domain;` |
| `namespace Persistence;` | `namespace ProjectName.Persistence;` |
| `namespace AntiCorruption;` | `namespace ProjectName.AntiCorruption;` |
| `namespace MessageQueue;` | `namespace ProjectName.MessageQueue;` |
| `namespace CrossCutting;` | `namespace ProjectName.CrossCutting;` |

Or keep simplified namespaces (`Api`, `Domain`, `Application`, etc.) — this is a team preference.

#### 5. Update Scalar title

Open `src/Api/Configurations/Extensions/ScalarConfiguration.cs` and change:

```csharp
document.Info = new()
{
    Title = "{API_NAME}",
```

And also:

```csharp
.WithTitle($"{{API_NAME}} - {environment.EnvironmentName}")
```

#### 6. Update README

Replace **all** occurrences of `{API_NAME}` in this `README.md` with the actual project name.

You can use your editor's `Find & Replace` (usually `Ctrl+Shift+H`) with the following text:

- **Find:** `{API_NAME}`
- **Replace:** `MyNewProjectName`

#### 7. Review and commit

After all changes, run a full build to ensure everything compiles:

```bash
dotnet build
```

Then commit your changes:

```bash
git add .
git commit -m "refactor: rename template API to {API_NAME}"
```

---

### Quick Checklist

Use this checklist to ensure you didn't miss any step:

- [ ] Repository cloned and folder renamed to new API name
- [ ] `.sln` file renamed to `{API_NAME}.sln`
- [ ] `AssemblyName` updated in all `.csproj` files
- [ ] `RootNamespace` updated in all `.csproj` files
- [ ] Namespaces adjusted in source code (`src/`)
- [ ] Scalar `Title` updated in `ScalarConfiguration.cs`
- [ ] All `{API_NAME}` placeholders replaced in `README.md`
- [ ] `dotnet build` runs successfully with zero errors
- [ ] Changes committed to version control

---

## Docker

There are two Docker files in the `docker/` folder:

| File | Purpose |
|------|---------|
| `docker/Dockerfile` | Production/CI build |
| `docker/Local.Dockerfile` | Local development build |

> **Attention:** Both Dockerfiles are currently empty. When creating a new API from this template, fill them according to your build pipeline. Basic example:

```dockerfile
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Api/Api.csproj"
RUN dotnet build "src/Api/Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Api/Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
```

---

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a branch for your feature or fix: `git checkout -b feature/feature-name`.
3. Commit your changes with clear messages.
4. Open a Pull Request for review.

---

> **Note:** This is a base template. Feel free to add/remove layers, packages and configurations according to your business domain needs.
