using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetTourById;

public sealed class GetTourByIdQueryHandler : IRequestHandler<GetTourByIdQuery, TourResponse>
{
    private readonly ITourRepository _tourRepository;

    public GetTourByIdQueryHandler(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<TourResponse> Handle(GetTourByIdQuery request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException("Tour not found.");

        return new TourResponse(tour.Id, tour.VehicleId, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }
}
