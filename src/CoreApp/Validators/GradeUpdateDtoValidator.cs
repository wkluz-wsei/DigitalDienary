using CoreApp.Application.Dto.Grades;
using FluentValidation;

namespace CoreApp.Validators;

public class GradeUpdateDtoValidator : AbstractValidator<GradeUpdateDto>
{
    private static readonly double[] AllowedGradeValues = [2.0, 3.0, 3.5, 4.0, 4.5, 5.0];

    public GradeUpdateDtoValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data wystawienia oceny jest wymagana.")
            .LessThanOrEqualTo(_ => DateTime.Today).WithMessage("Data wystawienia oceny nie może być z przyszłości.");

        RuleFor(x => x.GradeValue)
            .Must(value => AllowedGradeValues.Contains(value))
            .WithMessage("Niepoprawna wartość oceny.");
    }
}
