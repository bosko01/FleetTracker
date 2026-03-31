using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Commands.DeleteVehicle;

public sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Unit>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        _vehicleRepository.SoftDelete(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
