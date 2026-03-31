# FleetTracker

Clean Architecture + CQRS ASP.NET Core Web API for fleet and tour tracking.

## Solution structure

- `src/FleetTracker.API`
- `src/FleetTracker.Application`
- `src/FleetTracker.Domain`
- `src/FleetTracker.Infrastructure`

## Notes

- Vehicles are soft deleted.
- Daily and total mileage are calculated from `Tours`.
- Use `dotnet ef migrations add InitialCreate --project src/FleetTracker.Infrastructure --startup-project src/FleetTracker.API` to create the initial migration.


## LLM documentation

- Detailed project documentation optimized for LLM context loading: `LLM_PROJECT_DOCUMENTATION.md`.

## Development Admin Login

In development, a default admin user is seeded from `AdminSeed` in `src/FleetTracker.API/appsettings.json` (default: `admin` / `Admin123!`).
Change these values before production.
