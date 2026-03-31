using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Queries.GetVehicleById;

public sealed class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleResponse>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleByIdQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleResponse> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        return new VehicleResponse(vehicle.Id, vehicle.RegistrationPlate, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PayloadCapacityKg, vehicle.HasRamp);
    }
}
