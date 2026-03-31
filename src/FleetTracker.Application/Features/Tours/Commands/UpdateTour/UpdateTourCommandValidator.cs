using FluentValidation;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed class UpdateTourCommandValidator : AbstractValidator<UpdateTourCommand>
{
    public UpdateTourCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.TourNumber).GreaterThan(0);
        RuleFor(x => x.UnloadCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}
