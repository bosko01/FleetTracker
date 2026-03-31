using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.UpdateVehicle;

public sealed record UpdateVehicleCommand(
    Guid Id,
    string RegistrationPlate,
    string Make,
    string Model,
    int Year,
    decimal PayloadCapacityKg,
    bool HasRamp,
    decimal InitialMileageKm) : IRequest<VehicleResponse>;
