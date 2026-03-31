using FluentValidation;

namespace FleetTracker.Application.Features.Vehicles.Commands.UpdateVehicle;

public sealed class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        var maxYear = DateTime.UtcNow.Year + 1;

        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RegistrationPlate).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Make).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(1950, maxYear);
        RuleFor(x => x.PayloadCapacityKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.InitialMileageKm).GreaterThanOrEqualTo(0);
    }
}
