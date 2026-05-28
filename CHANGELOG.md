# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.1.0] — 2026-05-28

### ADDED

- `README.md` — full template documentation: architecture, configuration, prerequisites, EF Core migrations guide, Docker, CI/CD, renaming guide, and Template Extension Points table.
- `CHANGELOG.md` — full release history.

### UPDATED

- `Api.csproj` version bumped to `2.1.0`.
- Applied package updates from `2.0.0` to `Api.csproj` (previously documented but not applied to the file): replaced `Swashbuckle.AspNetCore` with `Scalar.AspNetCore` `2.14.14`; `Asp.Versioning.Mvc` `8.1.1` → `10.0.0`; `Asp.Versioning.Mvc.ApiExplorer` `8.1.1` → `10.0.0`; `IATec.Shared.Api` `1.1.0` → `1.2.0`; `Microsoft.AspNetCore.OpenApi` `10.0.2` → `10.0.8`.

---

## [2.0.0] — 2026-05-26

### ADDED

- `ExceptionPipelineBehavior` from `IATec.Shared.Behaviors` registered in MediatR pipeline alongside `ValidatorPipelineBehavior`.
- `Microsoft.Extensions.Options` `10.0.8` added to `Application.csproj`.

### UPDATED

- **BREAKING:** Replaced `Swashbuckle.AspNetCore` with `Scalar.AspNetCore` `2.14.14`. Removed `SwaggerExtension.cs` and all Swagger configuration.
- **BREAKING:** `IATec.Shared.Application` `1.1.0` → `2.0.0`.
- **BREAKING:** `IATec.Shared.Domain` `1.2.0` → `2.0.1`.
- **BREAKING:** `IATec.Shared.Behaviors` `1.2.0` → `1.3.0`.
- **BREAKING:** `IATec.Shared.HttpClient` `2.1.0` → `3.0.0`.
- **BREAKING:** `CreateAssetCommand` and `CheckIfExistsAssetQuery` converted from `sealed class` to `readonly record struct`.
- `IATec.Shared.Api` `1.1.0` → `1.2.0`.
- `Microsoft.AspNetCore.OpenApi` `10.0.1` → `10.0.8`.
- `Asp.Versioning.Mvc` `8.1.1` → `10.0.0`.
- `Asp.Versioning.Mvc.ApiExplorer` `8.1.1` → `10.0.0`.
- `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Http` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Configuration` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Configuration.Binder` `10.0.1` → `10.0.8`.
- `launchUrl` in `launchSettings.json` changed from `swagger` to `documentation`.

### FIXED

- `LogDispatcher.cs`: `Content = content?.ToString()!` replaced with `Content = content?.ToString() ?? string.Empty`.

### REMOVED

- `Swashbuckle.AspNetCore` package and `SwaggerExtension.cs`.

---

## [1.3.0] — 2026-03-11

### ADDED

- PostgreSQL persistence layer:
  - `DatabaseExtension.cs` — registers `ReadDataContext` and `WriteDataContext` via Npgsql with snake_case naming convention.
  - `EntityFrameworkOption.cs` — typed options bound from `EntityFramework` section (`SensitiveDataLogging`).
  - `PostgreSqlOption.cs` — typed options bound from `PostgreSQL` section (Database, User, Password, ServerReader, ServerWriter, Port). Builds connection string via `GetConnectionString(isReader)`.
  - `ReadDataContext.cs` — EF Core DbContext with `NoTrackingWithIdentityResolution` for read queries.
  - `WriteDataContext.cs` — EF Core DbContext with default change tracking for write operations.
  - `PersonMapping.cs` — `IEntityTypeConfiguration<Person>` mapping value objects via `OwnsOne`, schema `people`, table `person`.
  - `DocumentMapping.cs` — `IEntityTypeConfiguration<Document>` mapping.
  - `20260121134832_Init.cs` — initial EF Core migration creating `people.person` table.
- `appsettings.json` updated with `PostgreSQL` and `EntityFramework` sections.
- Domain model — `People` aggregate: `Person` (aggregate root), `Document` (entity), `FirstNameValue`, `LastNameValue`, `MiddleNameValue`, `IssuerValue`, `ValueValue` (value objects).

---

## [1.2.0] — 2025-04-29

### UPDATED

- `ValidatorPipelineBehavior<TRequest, TResponse>` constraint changed from `where TResponse : Result` to `where TResponse : ResultBase, new()` — enables generic return values.

---

## [1.1.0] — 2024-09-23

### UPDATED

- `Microsoft.AspNetCore.OpenApi` `8.0.6` → `8.0.8`.
- `Swashbuckle.AspNetCore` `6.6.2` → `6.8.0`.
- `FluentResults` `3.15.2` → `3.16.0`.
- `FluentValidation` `11.9.1` → `11.10.0`.
- `FluentValidation.DependencyInjectionExtensions` `11.9.1` → `11.10.0`.
- `MediatR` `12.2.0` → `12.4.1`.

---

## [1.0.0] — 2024-05-28

### ADDED

- Initial project structure forked from `IATec.Standard.Net.Api`:
  - **Api** — ASP.NET Core entrypoint with `Program.cs`, `ApiDependencyInjectionConfig.cs`, configuration extensions, and `launchSettings.json`.
  - **Application** — `ApplicationDependencyInjectionConfig.cs`, `MediatorConfig`, `ValidatorConfig`, `ValidatorFactory`, `LogDispatcher`, Assets feature stubs.
  - **CrossCutting** — `ValidatorPipelineBehavior` (local).
  - **Domain** — Contracts, seedwork entities, shared options, error/success result types.
  - **Persistence** — `PersistenceDependencyInjectionConfig.cs`, empty `DatabaseExtension` (prepared for PostgreSQL).
  - **AntiCorruption** — `LoggingConfig`, `LogService`.
  - **MessageQueue** — `MessageQueueDependencyInjectionConfig.cs` (stub).
  - **Domain.Tests** and **Application.Tests** — empty test projects.
- `launchSettings.json` — port `5015`, environment `Local`.
- `appsettings.json` — `TimeZone`, `Container`, `Logging`.
- `docker/Dockerfile` and `docker/Local.Dockerfile` (empty).
- `secrets/secrets.yml` — Kubernetes Secret manifest template.
