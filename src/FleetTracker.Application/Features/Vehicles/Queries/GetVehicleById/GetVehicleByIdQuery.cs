using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Queries.GetVehicleById;

public sealed record GetVehicleByIdQuery(Guid Id) : IRequest<VehicleResponse>;
