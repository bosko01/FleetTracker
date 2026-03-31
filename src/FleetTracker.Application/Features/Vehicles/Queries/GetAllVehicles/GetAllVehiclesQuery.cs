using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Queries.GetAllVehicles;

public sealed record GetAllVehiclesQuery : IRequest<IReadOnlyList<VehicleListItemResponse>>;
