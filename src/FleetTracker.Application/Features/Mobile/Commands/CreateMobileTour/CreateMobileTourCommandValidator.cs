using FluentValidation;

namespace FleetTracker.Application.Features.Mobile.Commands.CreateMobileTour;

public sealed class CreateMobileTourCommandValidator : AbstractValidator<CreateMobileTourCommand>
{
    public CreateMobileTourCommandValidator()
    {
        RuleFor(x => x.DriverId).NotEmpty();
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.UnloadCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}
