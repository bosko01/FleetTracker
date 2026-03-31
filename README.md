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
