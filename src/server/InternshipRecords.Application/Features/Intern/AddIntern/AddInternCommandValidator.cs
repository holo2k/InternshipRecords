using FluentValidation;

namespace InternshipRecords.Application.Features.Intern.AddIntern;

public class AddInternCommandValidator : AbstractValidator<AddInternCommand>
{
    public AddInternCommandValidator()
    {
        RuleFor(x => x.Intern).NotNull().WithMessage("Модель стажёра обязательна");

        When(x => x.Intern != null, () =>
        {
            RuleFor(x => x.Intern.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

            RuleFor(x => x.Intern.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов");

            RuleFor(x => x.Intern.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email")
                .MaximumLength(200).WithMessage("Email не должен превышать 200 символов");

            RuleFor(x => x.Intern.Phone)
                .MaximumLength(20).WithMessage("Телефон не должен превышать 20 символов")
                .Matches(@"^\+?[0-9\s\-\(\)]*$")
                .When(cmd => !string.IsNullOrEmpty(cmd.Intern.Phone))
                .WithMessage("Некорректный формат телефона");

            RuleFor(x => x.Intern.BirthDate)
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-16)))
                .WithMessage("Стажёр должен быть старше 16 лет");
        });
    }
}