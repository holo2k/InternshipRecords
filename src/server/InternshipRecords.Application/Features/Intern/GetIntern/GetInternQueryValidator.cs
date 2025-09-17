using FluentValidation;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public class GetInternQueryValidator : AbstractValidator<GetInternQuery>
{
    public GetInternQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Идентификатор стажёра обязателен")
            .NotEqual(Guid.Empty).WithMessage("Некорректный идентификатор стажёра");
    }
}