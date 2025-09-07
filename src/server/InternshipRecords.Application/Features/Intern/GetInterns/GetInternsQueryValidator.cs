using FluentValidation;

namespace InternshipRecords.Application.Features.Intern.GetInterns;

public class GetInternsQueryValidator : AbstractValidator<GetInternsQuery>
{
    public GetInternsQueryValidator()
    {
        RuleFor(x => x.DirectionId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Некорректный идентификатор направления");

        RuleFor(x => x.ProjectId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Некорректный идентификатор проекта");
    }
}