using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetToursByVehicleAndDate;

public sealed class GetToursByVehicleAndDateQueryHandler : IRequestHandler<GetToursByVehicleAndDateQuery, IReadOnlyList<TourListItemResponse>>
{
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;

    public GetToursByVehicleAndDateQueryHandler(ITourRepository tourRepository, IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
    {
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<TourListItemResponse>> Handle(GetToursByVehicleAndDateQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        var drivers = (await _driverRepository.GetAllAsync(cancellationToken)).ToDictionary(x => x.Id);
        var tours = await _tourRepository.GetByVehicleAndDateAsync(request.VehicleId, request.Date, cancellationToken);
        return tours.Select(t => new TourListItemResponse(
            t.Id,
            t.VehicleId,
            vehicle.RegistrationPlate,
            t.DriverId,
            drivers.TryGetValue(t.DriverId, out var d) ? d.FullName : string.Empty,
            t.Date,
            t.TourNumber,
            t.UnloadCount,
            t.WeightKg,
            t.DistanceKm)).ToList();
    }
}
