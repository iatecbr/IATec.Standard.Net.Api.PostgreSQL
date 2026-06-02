# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.1.0] — 2026-05-28

### ADDED

- `README.md` — full template documentation: architecture, configuration, prerequisites, EF Core migrations guide, Docker, CI/CD, renaming guide, and Template Extension Points table.
- `CHANGELOG.md` — full release history.
- `AGENTS.md` — agent instruction file with developer commands, architecture conventions, persistence gotchas, and renaming checklist for OpenCode sessions.
- `ScalarExtension.cs` — replaces `SwaggerExtension.cs`; wires `AddOpenApi()` and `MapScalarApiReference("/documentation")`.

### UPDATED

- `Api.csproj` version bumped to `2.1.0`.
- Applied package updates merged from upstream (`IATec.Standard.Net.Api` `2.0.0`):
  - Replaced `Swashbuckle.AspNetCore` with `Scalar.AspNetCore` `2.14.14` + `Microsoft.AspNetCore.OpenApi` `10.0.8`.
  - `IATec.Shared.Application` `1.1.0` → `2.0.0`.
  - `IATec.Shared.Domain` `1.2.0` → `2.0.1`.
  - `IATec.Shared.Behaviors` `1.2.0` → `1.3.0`.
  - `IATec.Shared.HttpClient` `2.1.0` → `3.0.0`.
  - `MediatR` `14.0.0` → `14.1.0`.
  - `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.1` → `10.0.8`.
  - `Microsoft.Extensions.Http` `10.0.1` → `10.0.8`.
  - `Microsoft.Extensions.Configuration` `10.0.2` → `10.0.8`.
  - `Microsoft.Extensions.Configuration.Binder` `10.0.2` → `10.0.8`.
  - `Microsoft.Extensions.Options` `10.0.8` added to `Application.csproj`.
  - `Asp.Versioning.Mvc` `8.1.1` → `10.0.0`; `Asp.Versioning.Mvc.ApiExplorer` `8.1.1` → `10.0.0`.
  - `IATec.Shared.Api` `1.1.0` → `1.2.0`.
  - `launchUrl` in `launchSettings.json` changed from `swagger` to `documentation`.
- `ApiDependencyInjectionConfig.cs` — replaced `AddSwagger()`/`UseApiSwagger()` with `AddOpenApiConfig()`/`ConfigureOpenApi()`; Scalar UI now guarded by `!IsProduction()`.
- `CreateAssetCommand` and `CheckIfExistsAssetQuery` converted from `sealed class` to `readonly record struct`.
- `LogDispatcher.cs`: `Content = content?.ToString()!` replaced with `Content = content?.ToString() ?? string.Empty`.

### REMOVED

- `SwaggerExtension.cs` — replaced by `ScalarExtension.cs`.

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
