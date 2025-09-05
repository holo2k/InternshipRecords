using FluentValidation;

namespace InternshipRecords.Application.Features.Direction.GetDirections;

public class GetDirectionsQueryValidator : AbstractValidator<GetDirectionsQuery>
{
    private static readonly string[] AllowedParams = { "orderByName", "orderByCount" };

    public GetDirectionsQueryValidator()
    {
        RuleFor(x => x.QueryParams)
            .Must(queryParams =>
                queryParams == null || queryParams.All(p => AllowedParams.Contains(p)))
            .WithMessage("Такого параметра сортировки не существует.");
    }
}