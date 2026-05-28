using CoreApp.Application.Authorization;
using CoreApp.Application.Repositories;
using CoreApp.Application.Security;
using CoreApp.Application.Services;
using CoreApp.Application.UnitOfWork;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Memory;
using Infrastructure.Seeders;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddUniversityEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStudentRepository, EfStudentRepository>();
        services.AddScoped<ILecturerRepository, EfLecturerRepository>();
        services.AddScoped<ICourseRepository, EfCourseRepository>();
        services.AddScoped<IAcademicYearRepository, EfAcademicYearRepository>();
        services.AddScoped<IGradeRepository, EfGradeRepository>();
        services.AddScoped<IUniversityUnitOfWork, EfUniversityUnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDataSeeder, IdentityDbSeeder>();
        services.AddScoped<IDataSeeder, UniversityDbSeeder>();

        services.AddDbContext<UniversityDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("UniversityDb")));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<UniversityDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddUniversityMemoryModule(
        this IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, MemoryStudentRepository>();
        services.AddSingleton<ILecturerRepository, MemoryLecturerRepository>();
        services.AddSingleton<ICourseRepository, MemoryCourseRepository>();
        services.AddSingleton<IAcademicYearRepository, MemoryAcademicYearRepository>();
        services.AddSingleton<IGradeRepository, MemoryGradeRepository>();
        services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();

        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly.Name(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));

            options.AddPolicy(AppPolicies.Administrator.Name(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));

            options.AddPolicy(AppPolicies.DeanOfficeOnly.Name(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString(), UserRole.DeanOfficeStaff.ToString()));

            options.AddPolicy(AppPolicies.LecturerOrAdmin.Name(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString(), UserRole.Lecturer.ToString()));

            options.AddPolicy(AppPolicies.ActiveUser.Name(), policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("status", SystemUserStatus.Active.ToString()));

            options.AddPolicy(AppPolicies.SalesDepartment.Name(), policy =>
                policy.RequireClaim("department", "Sales"));

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
