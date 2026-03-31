using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed class UpdateTourCommandHandler : IRequestHandler<UpdateTourCommand, TourResponse>
{
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourCommandHandler(ITourRepository tourRepository, IVehicleRepository vehicleRepository, IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourResponse> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException("Tour not found.");

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        var driver = await _driverRepository.GetByIdAsync(request.DriverId, cancellationToken)
                     ?? throw new NotFoundException("Driver not found.");

        if (!driver.IsActive)
            throw new ConflictException("Driver is inactive.");

        var nextTourNumber = tour.TourNumber;
        var targetChanged = tour.VehicleId != request.VehicleId || tour.Date != request.Date;
        if (targetChanged)
        {
            var currentNumberTaken = await _tourRepository.ExistsByVehicleDateAndTourNumberAsync(request.VehicleId, request.Date, tour.TourNumber, tour.Id, cancellationToken);
            if (currentNumberTaken)
            {
                nextTourNumber = await _tourRepository.GetMaxTourNumberForVehicleAndDateAsync(request.VehicleId, request.Date, cancellationToken) + 1;
                while (await _tourRepository.ExistsByVehicleDateAndTourNumberAsync(request.VehicleId, request.Date, nextTourNumber, tour.Id, cancellationToken))
                    nextTourNumber++;
            }
        }

        tour.Update(request.VehicleId, request.DriverId, request.Date, nextTourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm);
        _tourRepository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourResponse(tour.Id, tour.VehicleId, vehicle.RegistrationPlate, tour.DriverId, driver.FullName, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }
}
