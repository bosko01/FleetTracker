using FluentValidation;

namespace FleetTracker.Application.Features.Drivers.Commands.UpdateDriver;

public sealed class UpdateDriverCommandValidator : AbstractValidator<UpdateDriverCommand>
{
    public UpdateDriverCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
    }
}
