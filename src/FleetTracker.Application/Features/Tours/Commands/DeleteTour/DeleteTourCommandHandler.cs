using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.DeleteTour;

public sealed class DeleteTourCommandHandler : IRequestHandler<DeleteTourCommand, Unit>
{
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTourCommandHandler(ITourRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTourCommand request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException("Tour not found.");

        _tourRepository.Remove(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
