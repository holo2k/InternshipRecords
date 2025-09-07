using FluentValidation;

namespace InternshipRecords.Application.Features.Project.AddProject;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
{
    public AddProjectCommandValidator()
    {
        RuleFor(x => x.Project)
            .NotNull().WithMessage("Проект обязателен");

        RuleFor(x => x.Project.Name)
            .NotEmpty().WithMessage("Название проекта обязательно")
            .MaximumLength(100).WithMessage("Название проекта не должно превышать 100 символов");

        RuleFor(x => x.Project.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов");
    }
}