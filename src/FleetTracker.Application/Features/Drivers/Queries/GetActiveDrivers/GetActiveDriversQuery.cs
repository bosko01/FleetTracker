using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetActiveDrivers;

public sealed record GetActiveDriversQuery : IRequest<IReadOnlyList<DriverListItemResponse>>;
