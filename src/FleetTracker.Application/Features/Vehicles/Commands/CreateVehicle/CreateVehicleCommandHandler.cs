using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Vehicles.Responses;
using FleetTracker.Domain.Entities;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.CreateVehicle;

public sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleResponse> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        if (await _vehicleRepository.ExistsByRegistrationPlateAsync(request.RegistrationPlate, null, cancellationToken))
            throw new ConflictException("A vehicle with the same registration plate already exists.");

        var vehicle = Vehicle.Create(request.RegistrationPlate, request.Make, request.Model, request.Year, request.PayloadCapacityKg, request.HasRamp, request.InitialMileageKm, DateTime.UtcNow);

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VehicleResponse(vehicle.Id, vehicle.RegistrationPlate, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PayloadCapacityKg, vehicle.HasRamp, vehicle.InitialMileageKm, vehicle.InitialMileageRecordedAtUtc);
    }
}
