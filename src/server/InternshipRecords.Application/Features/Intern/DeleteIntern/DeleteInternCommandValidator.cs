using FluentValidation;

namespace InternshipRecords.Application.Features.Intern.DeleteIntern;

public class DeleteInternCommandValidator : AbstractValidator<DeleteInternCommand>
{
    public DeleteInternCommandValidator()
    {
        RuleFor(x => x.InternId)
            .NotEmpty().WithMessage("Идентификатор стажёра обязателен")
            .NotEqual(Guid.Empty).WithMessage("Некорректный идентификатор стажёра");
    }
}