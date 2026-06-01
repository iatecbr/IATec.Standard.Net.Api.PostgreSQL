# AGENTS.md

This is `IATec.Standard.Net.Api.PostgreSQL` — a scaffolding template (v2.1.0) for new .NET 10 APIs at IATec.
Clean Architecture / Vertical Slices with MediatR CQRS, PostgreSQL via EF Core.
**It is a blank template.** Controllers, handlers, and test bodies are intentionally empty stubs.

---

## Key Toolchain

| Concern | Detail |
|---|---|
| Runtime | .NET 10.0 (`net10.0`) |
| ORM | EF Core 10.0.2 + `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.0 |
| Naming | `EFCore.NamingConventions` — all DB identifiers are `snake_case` |
| CQRS | MediatR 14.1.0 |
| Validation | FluentValidation 12.1.1 |
| Result type | FluentResults 4.0.0 — handlers return `Result` / `Result<T>` |
| API docs | **Scalar** via `ScalarExtension.cs` — UI at `/documentation`; OpenAPI JSON at `/openapi/v1.json` |

---

## Developer Commands

```bash
# Restore & build
dotnet restore
dotnet build IATec.Standard.Net.Api.sln

# Run locally (port 5015, env=Local, opens /documentation)
dotnet run --project src/Api/Api.csproj

# Run tests (no tests exist yet — projects are empty stubs)
dotnet test
dotnet test src/Domain.Tests/Domain.Tests.csproj
dotnet test --filter "FullyQualifiedName~MyTest"

# Add a migration (always target WriteDataContext)
dotnet ef migrations add <MigrationName> \
  --project src/Persistence \
  --startup-project src/Api \
  --context WriteDataContext

# Apply migrations manually (required in Local — auto-apply is skipped there)
dotnet ef database update \
  --project src/Persistence \
  --startup-project src/Api \
  --context WriteDataContext
```

No linting, formatting, or codegen tooling is configured (`.editorconfig` is 0 bytes).

---

## Project Structure

```
src/
  Api/              # Entrypoint — ASP.NET Core Web API (port 5015)
  Application/      # MediatR handlers, validators, dispatchers
  Domain/           # People aggregate: Person, Document entities; value objects
  Persistence/      # EF Core contexts, mappings, migrations
  AntiCorruption/   # Typed HttpClient → IATec Log Service
  CrossCutting/     # Empty stub
  MessageQueue/     # Empty stub (ConfigureMessageQueue is a no-op)
  Domain.Tests/     # Empty — no test framework packages yet
  Application.Tests/# Empty — no test framework packages yet
docker/             # Both Dockerfiles are 0 bytes — must be written from scratch
secrets/            # Kubernetes Secret template with unfilled placeholders
```

---

## Persistence / Database

- Two `DbContext`s: `ReadDataContext` (no-tracking) and `WriteDataContext` (change-tracked, owns migrations).
- Separate `ServerReader` / `ServerWriter` connection strings — designed for read-replica split.
- Connection string assembled from `appsettings.json` `PostgreSQL` section; **`Password` is blank by default** — the app will not connect without a password. No `appsettings.Local.json` exists; use user secrets or env vars.
- **Auto-migration is skipped when `EnvironmentName == "Local"`** — always run `dotnet ef database update` manually after adding migrations locally.
- Migrations folder is `src/Persistence/MIgrations/` (capital `I` — typo baked into `Persistence.csproj`). Do not rename without updating the project file.
- Schema: `people`, table: `person`.

---

## Architecture Conventions

### MediatR pipeline order (registered in `CrossCutting`)
1. `ValidatorPipelineBehavior<,>` — FluentValidation before handler
2. `ExceptionPipelineBehavior<,>` — global exception wrapper

### Domain model
- Aggregate roots extend `EntityUlidInt32` — provides both `int32 Id` and `char(26) ExternalId` (ULID).
- Value objects are plain classes, not records.
- Private constructors; factory methods on entities (`Document.Create(...)`).
- Collections exposed as `IReadOnlyCollection<T>` backed by private `List<T>`.

### Environments
- `Local` / `Development` — Scalar UI active at `/documentation`.
- `Local` — auto-migration skipped.
- `Production` — Scalar UI hidden (`!app.Environment.IsProduction()` guard in `ApiDependencyInjectionConfig.cs`).

---

## Gotchas

1. **`SensitiveDataLogging: true` in default config** — must be `false` before any non-local deployment.
2. **CORS is fully open** (`AllowAnyOrigin/Method/Header`) — restrict before production.
3. **No auth middleware** — `UseAuthentication()` / `UseAuthorization()` are not called; add explicitly if needed.
4. **`LogServiceOption` and `ContainerOption` sections are missing from `appsettings.json`** — Log Service calls will fail silently (exceptions are swallowed).
5. **`{API_NAME}` placeholders** remain in `ScalarExtension.cs`, README, and `secrets/secrets.yml` — replace when cloning as a new API.
6. **No CI/CD** — no `.github/workflows/`, no Docker, no Kubernetes manifests ready to use.
7. **`Controllers/` is empty** — no endpoints exist; all feature handlers throw `NotImplementedException`.

---

## Renaming When Cloning as a New API

- Rename `.sln` and update `<AssemblyName>` / `<RootNamespace>` in `Api.csproj`.
- Replace namespaces across all `src/` projects.
- Update `PostgreSQL.Database` in `appsettings.json`.
- Update schema/table names in `PersonMapping.cs` / `DocumentMapping.cs`.
- Replace `{API_NAME}` in `ScalarExtension.cs` (two occurrences).
- Set `<Version>` to `1.0.0` in `Api.csproj`.
- Add `LogServiceOption` and `ContainerOption` sections to `appsettings.json`.
- Write `docker/Dockerfile` and `docker/Local.Dockerfile` (both are 0 bytes).
- Add test framework packages to `Domain.Tests` and `Application.Tests`.
