using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using MediatR;

namespace FleetTracker.Application.Features.Drivers.Commands.DeactivateDriver;

public sealed class DeactivateDriverCommandHandler : IRequestHandler<DeactivateDriverCommand>
{
    private readonly IDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Driver not found.");
        driver.Deactivate();
        _driverRepository.Update(driver);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
