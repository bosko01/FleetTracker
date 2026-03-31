using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed class UpdateTourCommandHandler : IRequestHandler<UpdateTourCommand, TourResponse>
{
    private readonly ITourRepository _tourRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourCommandHandler(ITourRepository tourRepository, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourResponse> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException("Tour not found.");

        var vehicle = await _vehicleRepository.GetByIdAsync(tour.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        if (await _tourRepository.ExistsByVehicleDateAndTourNumberAsync(tour.VehicleId, request.Date, request.TourNumber, tour.Id, cancellationToken))
            throw new ConflictException("Tour number already exists for this vehicle and date.");

        tour.Update(request.Date, request.TourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm);
        _tourRepository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourResponse(tour.Id, tour.VehicleId, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }
}
