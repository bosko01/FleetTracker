namespace FleetTracker.Application.Features.Statistics.Responses;

public sealed record VehicleDailySummaryResponse(
    Guid VehicleId,
    string RegistrationPlate,
    DateOnly Date,
    int TourCount,
    int TotalUnloadCount,
    decimal TotalWeightKg,
    decimal TotalDistanceKm);
