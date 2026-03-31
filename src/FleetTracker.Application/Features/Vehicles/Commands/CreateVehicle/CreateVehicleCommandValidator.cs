using FluentValidation;

namespace FleetTracker.Application.Features.Vehicles.Commands.CreateVehicle;

public sealed class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        var maxYear = DateTime.UtcNow.Year + 1;

        RuleFor(x => x.RegistrationPlate).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Make).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(1950, maxYear);
        RuleFor(x => x.PayloadCapacityKg).GreaterThanOrEqualTo(0);
    }
}
