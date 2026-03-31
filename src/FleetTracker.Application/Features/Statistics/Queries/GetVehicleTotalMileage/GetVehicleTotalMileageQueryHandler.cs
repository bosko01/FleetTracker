using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Statistics.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Statistics.Queries.GetVehicleTotalMileage;

public sealed class GetVehicleTotalMileageQueryHandler : IRequestHandler<GetVehicleTotalMileageQuery, VehicleTotalMileageResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITourRepository _tourRepository;

    public GetVehicleTotalMileageQueryHandler(IVehicleRepository vehicleRepository, ITourRepository tourRepository)
    {
        _vehicleRepository = vehicleRepository;
        _tourRepository = tourRepository;
    }

    public async Task<VehicleTotalMileageResponse> Handle(GetVehicleTotalMileageQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        var totalDistance = await _tourRepository.GetTotalDistanceByVehicleAsync(request.VehicleId, cancellationToken);

        return new VehicleTotalMileageResponse(vehicle.Id, vehicle.RegistrationPlate, totalDistance);
    }
}
