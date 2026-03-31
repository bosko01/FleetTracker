namespace FleetTracker.Application.Features.Vehicles.Responses;

public sealed record VehicleResponse(
    Guid Id,
    string RegistrationPlate,
    string Make,
    string Model,
    int Year,
    decimal PayloadCapacityKg,
    bool HasRamp,
    decimal InitialMileageKm,
    DateTime InitialMileageRecordedAtUtc);
