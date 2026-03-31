using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetTodayToursByDriver;

public sealed class GetTodayToursByDriverQueryHandler : IRequestHandler<GetTodayToursByDriverQuery, IReadOnlyList<MobileTourListItemResponse>>
{
    private readonly IDriverRepository _driverRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public GetTodayToursByDriverQueryHandler(IDriverRepository driverRepository, ITourRepository tourRepository, IVehicleRepository vehicleRepository)
    {
        _driverRepository = driverRepository;
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<MobileTourListItemResponse>> Handle(GetTodayToursByDriverQuery request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.DriverId, cancellationToken) ?? throw new NotFoundException("Driver not found.");
        if (!driver.IsActive)
            throw new NotFoundException("Driver not found.");

        var vehicles = (await _vehicleRepository.GetAllAsync(cancellationToken)).ToDictionary(v => v.Id);
        var tours = await _tourRepository.GetByDriverAndDateAsync(request.DriverId, request.Date, cancellationToken);
        return tours.Select(t => new MobileTourListItemResponse(t.Id, t.Date, t.VehicleId, vehicles.TryGetValue(t.VehicleId, out var v) ? v.RegistrationPlate : string.Empty, t.TourNumber, t.UnloadCount, t.WeightKg, t.DistanceKm)).ToList();
    }
}
