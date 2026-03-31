namespace FleetTracker.Application.Features.Tours.Responses;

public sealed record TourListItemResponse(
    Guid Id,
    Guid VehicleId,
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
