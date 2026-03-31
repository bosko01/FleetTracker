using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.CreateVehicle;

public sealed record CreateVehicleCommand(
    string RegistrationPlate,
    string Make,
    string Model,
    int Year,
    decimal PayloadCapacityKg,
    bool HasRamp) : IRequest<VehicleResponse>;
