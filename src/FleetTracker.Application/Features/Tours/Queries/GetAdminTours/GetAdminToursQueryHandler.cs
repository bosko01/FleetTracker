using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetAdminTours;

public sealed class GetAdminToursQueryHandler : IRequestHandler<GetAdminToursQuery, IReadOnlyList<TourListItemResponse>>
{
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;

    public GetAdminToursQueryHandler(ITourRepository tourRepository, IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
    {
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<TourListItemResponse>> Handle(GetAdminToursQuery request, CancellationToken cancellationToken)
    {
        var vehicles = (await _vehicleRepository.GetAllAsync(cancellationToken)).ToDictionary(v => v.Id);
        var drivers = (await _driverRepository.GetAllAsync(cancellationToken)).ToDictionary(d => d.Id);

        var tours = await _tourRepository.GetFilteredAsync(request.Date, request.VehicleId, request.DriverId, cancellationToken);
        return tours.Select(t => new TourListItemResponse(
            t.Id,
            t.VehicleId,
            vehicles.TryGetValue(t.VehicleId, out var vehicle) ? vehicle.RegistrationPlate : string.Empty,
            t.DriverId,
            drivers.TryGetValue(t.DriverId, out var driver) ? driver.FullName : string.Empty,
            t.Date,
            t.TourNumber,
            t.UnloadCount,
            t.WeightKg,
            t.DistanceKm)).ToList();
    }
}
