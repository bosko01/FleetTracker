using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Drivers.Responses;
using FleetTracker.Domain.Entities;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.CreateDriver;

public sealed class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, DriverResponse>
{
    private readonly IDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DriverResponse> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = Driver.Create(request.FullName);
        await _driverRepository.AddAsync(driver, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new DriverResponse(driver.Id, driver.FullName, driver.IsActive);
    }
}
