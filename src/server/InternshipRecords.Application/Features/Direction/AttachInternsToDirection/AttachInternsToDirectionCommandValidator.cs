using FluentValidation;

namespace InternshipRecords.Application.Features.Direction.AttachInternsToDirection;

public class AttachInternsToDirectionCommandValidator : AbstractValidator<AttachInternsToDirectionCommand>
{
    public AttachInternsToDirectionCommandValidator()
    {
        RuleFor(x => x.DirectionId)
            .NotEmpty().WithMessage("Идентификатор направления обязателен.");

        RuleFor(x => x.InternIds)
            .NotNull().WithMessage("Список стажеров обязателен.")
            .Must(ids => ids.Length > 0).WithMessage("Необходимо указать хотя бы одного стажера.");

        RuleForEach(x => x.InternIds)
            .NotEmpty().WithMessage("Идентификатор стажера не может быть пустым");
    }
}