using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetDriverById;

public sealed record GetDriverByIdQuery(Guid Id) : IRequest<DriverResponse>;
