using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.ActivateDriver;

public sealed record ActivateDriverCommand(Guid Id) : IRequest;
