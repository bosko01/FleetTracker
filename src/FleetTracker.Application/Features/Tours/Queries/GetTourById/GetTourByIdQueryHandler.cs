using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetTourById;

public sealed class GetTourByIdQueryHandler : IRequestHandler<GetTourByIdQuery, TourResponse>
{
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;

    public GetTourByIdQueryHandler(ITourRepository tourRepository, IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
    {
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
    }

    public async Task<TourResponse> Handle(GetTourByIdQuery request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException("Tour not found.");

        var vehicle = await _vehicleRepository.GetByIdAsync(tour.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        var driver = await _driverRepository.GetByIdAsync(tour.DriverId, cancellationToken)
                     ?? throw new NotFoundException("Driver not found.");

        return new TourResponse(tour.Id, tour.VehicleId, vehicle.RegistrationPlate, tour.DriverId, driver.FullName, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }
}
