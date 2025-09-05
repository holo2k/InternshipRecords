using FluentValidation;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public class AddDirectionCommandValidator : AbstractValidator<AddDirectionCommand>
{
    public AddDirectionCommandValidator()
    {
        RuleFor(x => x.Direction)
            .NotNull().WithMessage("Направление обязательно");

        RuleFor(x => x.Direction.Name)
            .NotEmpty().WithMessage("Название направления обязательно")
            .MaximumLength(100).WithMessage("Название направления не должно превышать 100 символов");

        RuleFor(x => x.Direction.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов");
    }
}