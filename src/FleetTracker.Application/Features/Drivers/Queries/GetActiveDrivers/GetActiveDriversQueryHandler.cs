using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetActiveDrivers;

public sealed class GetActiveDriversQueryHandler : IRequestHandler<GetActiveDriversQuery, IReadOnlyList<DriverListItemResponse>>
{
    private readonly IDriverRepository _driverRepository;

    public GetActiveDriversQueryHandler(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<DriverListItemResponse>> Handle(GetActiveDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverRepository.GetActiveAsync(cancellationToken);
        return drivers.Select(d => new DriverListItemResponse(d.Id, d.FullName, d.IsActive)).ToList();
    }
}
