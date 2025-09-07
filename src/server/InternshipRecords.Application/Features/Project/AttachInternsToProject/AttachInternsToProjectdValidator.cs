using FluentValidation;

namespace InternshipRecords.Application.Features.Project.AttachInternsToProject;

public class AttachInternsToProjectCommandValidator : AbstractValidator<AttachInternsToProjectCommand>
{
    public AttachInternsToProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Идентификатор проекта обязателен.");

        RuleFor(x => x.InternIds)
            .NotNull().WithMessage("Список стажеров обязателен.")
            .Must(ids => ids.Length > 0).WithMessage("Необходимо указать хотя бы одного стажера.");

        RuleForEach(x => x.InternIds)
            .NotEmpty().WithMessage("Идентификатор стажера не может быть пустым");
    }
}