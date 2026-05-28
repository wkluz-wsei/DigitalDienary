using CoreApp.Application.Dto.Students;
using FluentValidation;

namespace CoreApp.Validators;

public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię nie może przekraczać 100 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imię zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie może przekraczać 200 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("Numer PESEL jest wymagany.")
            .Length(11).WithMessage("Numer PESEL musi składać się z 11 cyfr.")
            .Matches(@"^\d{11}$").WithMessage("Numer PESEL może zawierać wyłącznie cyfry.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy format adresu email.")
            .MaximumLength(200).WithMessage("Email nie może przekraczać 200 znaków.");

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
