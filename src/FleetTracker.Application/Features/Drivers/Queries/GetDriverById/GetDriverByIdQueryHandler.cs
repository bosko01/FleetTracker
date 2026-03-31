using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Queries.GetDriverById;

public sealed class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, DriverResponse>
{
    private readonly IDriverRepository _driverRepository;

    public GetDriverByIdQueryHandler(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<DriverResponse> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Driver not found.");
        return new DriverResponse(driver.Id, driver.FullName, driver.IsActive);
    }
}
