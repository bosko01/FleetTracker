using FleetTracker.Application.Features.Tours.Commands.CreateTour;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Commands.CreateMobileTour;

public sealed class CreateMobileTourCommandHandler : IRequestHandler<CreateMobileTourCommand, TourResponse>
{
    private readonly IMediator _mediator;

    public CreateMobileTourCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<TourResponse> Handle(CreateMobileTourCommand request, CancellationToken cancellationToken) =>
        _mediator.Send(new CreateTourCommand(request.VehicleId, request.DriverId, request.Date, request.UnloadCount, request.WeightKg, request.DistanceKm), cancellationToken);
}
