using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveVehicles;

public sealed class GetMobileActiveVehiclesQueryHandler : IRequestHandler<GetMobileActiveVehiclesQuery, IReadOnlyList<MobileVehicleListItemResponse>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetMobileActiveVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<MobileVehicleListItemResponse>> Handle(GetMobileActiveVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
        return vehicles.Select(v => new MobileVehicleListItemResponse(v.Id, v.RegistrationPlate, v.Make, v.Model, v.HasRamp)).ToList();
    }
}
