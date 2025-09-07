using FluentValidation;

namespace InternshipRecords.Application.Features.Project.GetProjects;

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    private static readonly string[] AllowedParams = { "orderByName", "orderByCount" };

    public GetProjectsQueryValidator()
    {
        RuleFor(x => x.QueryParams)
            .Must(queryParams =>
                queryParams == null || queryParams.All(p => AllowedParams.Contains(p)))
            .WithMessage("Такого параметра сортировки не существует.");
    }
}