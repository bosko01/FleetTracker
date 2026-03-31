namespace FleetTracker.API.Contracts;

public sealed record CreateTourRequest(
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);

public sealed record UpdateTourRequest(
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
