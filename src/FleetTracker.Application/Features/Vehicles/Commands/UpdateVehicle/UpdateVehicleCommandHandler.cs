using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.UpdateVehicle;

public sealed class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, VehicleResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleResponse> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (await _vehicleRepository.ExistsByRegistrationPlateAsync(request.RegistrationPlate, request.Id, cancellationToken))
            throw new ConflictException("A vehicle with the same registration plate already exists.");

        vehicle.UpdateDetails(
            request.RegistrationPlate,
            request.Make,
            request.Model,
            request.Year,
            request.PayloadCapacityKg,
            request.HasRamp,
            request.InitialMileageKm);

        _vehicleRepository.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VehicleResponse(vehicle.Id, vehicle.RegistrationPlate, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PayloadCapacityKg, vehicle.HasRamp, vehicle.InitialMileageKm, vehicle.InitialMileageRecordedAtUtc);
    }
}
