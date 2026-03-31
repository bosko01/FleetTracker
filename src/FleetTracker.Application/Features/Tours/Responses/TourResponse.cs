namespace FleetTracker.Application.Features.Tours.Responses;

public sealed record TourResponse(
    Guid Id,
    Guid VehicleId,
    string VehicleRegistrationPlate,
    Guid DriverId,
    string DriverFullName,
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm);
