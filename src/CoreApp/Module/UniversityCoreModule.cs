using CoreApp.Application.Services;
using CoreApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApp.Module;

public static class UniversityCoreModule
{
    public static IServiceCollection AddUniversityCoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentUpdateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<PersonCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<GradeDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<GradeUpdateDtoValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}
