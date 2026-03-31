using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.DeleteTour;

public sealed record DeleteTourCommand(Guid Id) : IRequest<Unit>;
