using FluentValidation;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public class DeleteDirectionCommandValidator : AbstractValidator<DeleteDirectionCommand>
{
    public DeleteDirectionCommandValidator()
    {
        RuleFor(x => x.DirectionId).NotEmpty().WithMessage("Идентификатор направления обязателен для удаления.");
    }
}