using FluentValidation;

namespace FleetTracker.Application.Features.Drivers.Commands.CreateDriver;

public sealed class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
    }
}
