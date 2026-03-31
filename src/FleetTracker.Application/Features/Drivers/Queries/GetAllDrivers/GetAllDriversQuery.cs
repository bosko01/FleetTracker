using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetAllDrivers;

public sealed record GetAllDriversQuery : IRequest<IReadOnlyList<DriverListItemResponse>>;
