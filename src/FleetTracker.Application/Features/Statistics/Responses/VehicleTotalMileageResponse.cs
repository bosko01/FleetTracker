namespace FleetTracker.Application.Features.Statistics.Responses;

public sealed record VehicleTotalMileageResponse(
    Guid VehicleId,
    string RegistrationPlate,
    decimal TotalDistanceKm);
