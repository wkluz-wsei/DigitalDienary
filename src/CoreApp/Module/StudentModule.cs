using CoreApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApp.Module;

public static class StudentModule
{
    public static IServiceCollection AddStudentsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentUpdateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<PersonCreateDtoValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
