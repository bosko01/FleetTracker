using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.DeactivateDriver;

public sealed record DeactivateDriverCommand(Guid Id) : IRequest;
