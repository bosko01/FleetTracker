using FluentValidation;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed class UpdateTourCommandValidator : AbstractValidator<UpdateTourCommand>
{
    public UpdateTourCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.DriverId).NotEmpty();
        RuleFor(x => x.UnloadCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}
