using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Tours.Responses;
using FleetTracker.Domain.Entities;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.CreateTour;

public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, TourResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTourCommandHandler(IVehicleRepository vehicleRepository, ITourRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourResponse> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        if (await _tourRepository.ExistsByVehicleDateAndTourNumberAsync(request.VehicleId, request.Date, request.TourNumber, null, cancellationToken))
            throw new ConflictException("Tour number already exists for this vehicle and date.");

        var tour = Tour.Create(request.VehicleId, request.Date, request.TourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm);
        await _tourRepository.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourResponse(tour.Id, tour.VehicleId, tour.Date, tour.TourNumber, tour.UnloadCount, tour.WeightKg, tour.DistanceKm);
    }
}
