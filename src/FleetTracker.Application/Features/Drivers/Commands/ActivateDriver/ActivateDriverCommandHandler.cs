using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.ActivateDriver;

public sealed class ActivateDriverCommandHandler : IRequestHandler<ActivateDriverCommand>
{
    private readonly IDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActivateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Driver not found.");
        driver.Activate();
        _driverRepository.Update(driver);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
