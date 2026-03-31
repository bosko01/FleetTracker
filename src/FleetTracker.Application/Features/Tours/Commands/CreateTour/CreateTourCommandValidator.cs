using FluentValidation;

namespace FleetTracker.Application.Features.Tours.Commands.CreateTour;

public sealed class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
{
    public CreateTourCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.DriverId).NotEmpty();
        RuleFor(x => x.UnloadCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}
