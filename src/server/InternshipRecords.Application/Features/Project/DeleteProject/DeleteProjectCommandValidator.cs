using FluentValidation;

namespace InternshipRecords.Application.Features.Project.DeleteProject;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Идентификатор проекта обязателен для удаления.");
    }
}