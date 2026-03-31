namespace FleetTracker.API.Contracts;

public sealed record CreateTourRequest(
    Guid DriverId,
    DateOnly Date,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);

public sealed record UpdateTourRequest(
    Guid VehicleId,
    Guid DriverId,
    DateOnly Date,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);

public sealed record CreateMobileTourRequest(
    Guid DriverId,
    Guid VehicleId,
    DateOnly Date,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
