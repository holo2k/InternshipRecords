using FluentValidation;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public class UpdateInternCommandValidator : AbstractValidator<UpdateInternCommand>
{
    public UpdateInternCommandValidator()
    {
        RuleFor(x => x.Intern)
            .NotNull()
            .WithMessage("Модель стажёра обязательна");

        RuleFor(x => x.Intern.Id)
            .NotEmpty()
            .WithMessage("Id обязателен для обновления");

        RuleFor(x => x.Intern.FirstName)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

        RuleFor(x => x.Intern.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна")
            .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов");

        RuleFor(x => x.Intern.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Некорректный формат Email");

        RuleFor(x => x.Intern.BirthDate)
            .LessThan(DateTime.UtcNow).WithMessage("Дата рождения не может быть в будущем");

        RuleFor(x => x.Intern.Phone)
            .MaximumLength(20).WithMessage("Телефон не должен превышать 20 символов");
    }
}