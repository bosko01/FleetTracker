namespace FleetTracker.Application.Features.Mobile.Responses;

public sealed record MobileTourListItemResponse(
    Guid Id,
    DateOnly Date,
    Guid VehicleId,
    string RegistrationPlate,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
