using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.CreateDriver;

public sealed record CreateDriverCommand(string FullName) : IRequest<DriverResponse>;
