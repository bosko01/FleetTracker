using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Vehicles.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Vehicles.Queries.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IReadOnlyList<VehicleListItemResponse>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetAllVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<VehicleListItemResponse>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
        return vehicles
            .Select(v => new VehicleListItemResponse(v.Id, v.RegistrationPlate, v.Make, v.Model, v.Year, v.PayloadCapacityKg, v.HasRamp))
            .ToList();
    }
}
