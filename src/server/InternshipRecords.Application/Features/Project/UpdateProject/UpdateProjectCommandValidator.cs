using FluentValidation;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Project)
            .NotNull().WithMessage("Проект обязателен");

        RuleFor(x => x.Project.Id)
            .NotEmpty().WithMessage("Id проекта обязательно");

        RuleFor(x => x.Project.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Имя не может быть длиннее 100 символов");

        RuleFor(x => x.Project.Description)
            .MaximumLength(500).WithMessage("Описание не может быть длиннее 500 символов");
    }
}