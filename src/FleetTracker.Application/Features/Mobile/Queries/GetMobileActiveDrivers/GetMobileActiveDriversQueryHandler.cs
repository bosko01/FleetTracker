using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveDrivers;

public sealed class GetMobileActiveDriversQueryHandler : IRequestHandler<GetMobileActiveDriversQuery, IReadOnlyList<MobileDriverListItemResponse>>
{
    private readonly IDriverRepository _driverRepository;

    public GetMobileActiveDriversQueryHandler(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<MobileDriverListItemResponse>> Handle(GetMobileActiveDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverRepository.GetActiveAsync(cancellationToken);
        return drivers.Select(d => new MobileDriverListItemResponse(d.Id, d.FullName)).ToList();
    }
}
