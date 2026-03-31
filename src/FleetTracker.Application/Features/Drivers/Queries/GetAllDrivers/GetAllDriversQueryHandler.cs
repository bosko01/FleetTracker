using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetAllDrivers;

public sealed class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, IReadOnlyList<DriverListItemResponse>>
{
    private readonly IDriverRepository _driverRepository;

    public GetAllDriversQueryHandler(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<DriverListItemResponse>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverRepository.GetAllAsync(cancellationToken);
        return drivers.Select(d => new DriverListItemResponse(d.Id, d.FullName, d.IsActive)).ToList();
    }
}
