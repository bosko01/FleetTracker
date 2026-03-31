namespace FleetTracker.API.Contracts;

public sealed record CreateVehicleRequest(
    string RegistrationPlate,
    string Make,
    string Model,
    int Year,
    decimal PayloadCapacityKg,
    bool HasRamp,
    decimal InitialMileageKm);

public sealed record UpdateVehicleRequest(
    string RegistrationPlate,
    string Make,
    string Model,
    int Year,
    decimal PayloadCapacityKg,
    bool HasRamp,
    decimal InitialMileageKm);
