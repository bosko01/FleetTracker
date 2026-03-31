namespace FleetTracker.Application.Features.Tours.Responses;

public sealed record TourResponse(
    Guid Id,
    Guid VehicleId,
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
