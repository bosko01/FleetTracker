using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Drivers.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.UpdateDriver;

public sealed class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, DriverResponse>
{
    private readonly IDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DriverResponse> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Driver not found.");
        driver.UpdateDetails(request.FullName);
        _driverRepository.Update(driver);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new DriverResponse(driver.Id, driver.FullName, driver.IsActive);
    }
}
