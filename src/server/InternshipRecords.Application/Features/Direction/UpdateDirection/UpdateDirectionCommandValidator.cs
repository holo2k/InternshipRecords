using FluentValidation;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public class UpdateDirectionCommandValidator : AbstractValidator<UpdateDirectionCommand>
{
    public UpdateDirectionCommandValidator()
    {
        RuleFor(x => x.Direction)
            .NotNull().WithMessage("Направление обязательно");

        RuleFor(x => x.Direction.Id)
            .NotEmpty().WithMessage("Id направления обязательно");

        RuleFor(x => x.Direction.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Имя не может быть длиннее 100 символов");

        RuleFor(x => x.Direction.Description)
            .MaximumLength(500).WithMessage("Описание не может быть длиннее 500 символов");
    }
}