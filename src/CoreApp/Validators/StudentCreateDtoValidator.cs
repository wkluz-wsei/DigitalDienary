using CoreApp.Application.Dto.Students;
using FluentValidation;

namespace CoreApp.Validators;

public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator()
    {
        RuleFor(x => x).SetValidator(new PersonCreateDtoValidator());

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Numer indeksu jest wymagany.")
            .MaximumLength(50).WithMessage("Numer indeksu nie może przekraczać 50 znaków.");

        RuleFor(x => x.ProgramName)
            .NotEmpty().WithMessage("Nazwa kierunku jest wymagana.")
            .MaximumLength(200).WithMessage("Nazwa kierunku nie może przekraczać 200 znaków.");

        RuleFor(x => x.YearOfStudy)
            .InclusiveBetween(1, 5)
            .WithMessage("Niepoprawny rok studiów.");
    }
}
