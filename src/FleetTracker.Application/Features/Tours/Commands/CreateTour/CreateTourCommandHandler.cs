using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using FleetTracker.Domain.Entities;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.CreateTour;

public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, TourResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTourCommandHandler(
        IVehicleRepository vehicleRepository,
        IDriverRepository driverRepository,
        ITourRepository tourRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourResponse> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        var driver = await _driverRepository.GetByIdAsync(request.DriverId, cancellationToken)
                     ?? throw new NotFoundException("Driver not found.");

        if (!driver.IsActive)
            throw new ConflictException("Driver is inactive.");

        var nextTourNumber = await GetNextTourNumberAsync(request.VehicleId, request.Date, cancellationToken);

        var tour = Tour.Create(request.VehicleId, request.DriverId, request.Date, nextTourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm);
        await _tourRepository.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourResponse(tour.Id, tour.VehicleId, vehicle.RegistrationPlate, tour.DriverId, driver.FullName, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }

    private async Task<int> GetNextTourNumberAsync(Guid vehicleId, DateOnly date, CancellationToken cancellationToken)
    {
        var next = await _tourRepository.GetMaxTourNumberForVehicleAndDateAsync(vehicleId, date, cancellationToken) + 1;
        while (await _tourRepository.ExistsByVehicleDateAndTourNumberAsync(vehicleId, date, next, null, cancellationToken))
            next++;

        return next;
    }
}
