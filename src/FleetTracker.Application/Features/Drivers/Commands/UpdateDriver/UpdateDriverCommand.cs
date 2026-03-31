using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.UpdateDriver;

public sealed record UpdateDriverCommand(Guid Id, string FullName) : IRequest<DriverResponse>;
