using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.DeleteVehicle;

public sealed record DeleteVehicleCommand(Guid Id) : IRequest<Unit>;
