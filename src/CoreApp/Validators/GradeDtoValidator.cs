using CoreApp.Application.Dto.Grades;
using FluentValidation;

namespace CoreApp.Validators;

public class GradeDtoValidator : AbstractValidator<GradeDto>
{
    private static readonly double[] AllowedGradeValues = [2.0, 3.0, 3.5, 4.0, 4.5, 5.0];

    public GradeDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Identyfikator kursu jest wymagany.");

        RuleFor(x => x.LecturerId)
            .NotEmpty().WithMessage("Identyfikator wykładowcy jest wymagany.");

        RuleFor(x => x.AcademicYearId)
            .NotEmpty().WithMessage("Identyfikator roku akademickiego jest wymagany.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data wystawienia oceny jest wymagana.")
            .LessThanOrEqualTo(_ => DateTime.Today).WithMessage("Data wystawienia oceny nie może być z przyszłości.");

        RuleFor(x => x.GradeValue)
            .Must(value => AllowedGradeValues.Contains(value))
            .WithMessage("Niepoprawna wartość oceny.");
    }
}
