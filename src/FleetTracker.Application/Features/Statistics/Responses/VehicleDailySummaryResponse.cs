namespace FleetTracker.Application.Features.Statistics.Responses;

public sealed record VehicleDailySummaryResponse(
    Guid VehicleId,
    string RegistrationPlate,
    DateOnly Date,
    int TotalTours,
    int TotalUnloads,
    decimal TotalWeightKg,
    decimal TotalDistanceKm,
    int DistinctDriversCount);
