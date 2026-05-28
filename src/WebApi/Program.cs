using CoreApp.Module;
using Infrastructure;
using Infrastructure.Security;
using WebApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddJwt(new JwtSettings(builder.Configuration));
builder.Services.AddUniversityEfModule(builder.Configuration);
builder.Services.AddUniversityCoreModule(builder.Configuration);
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeders = scope.ServiceProvider
        .GetServices<IDataSeeder>()
        .OrderBy(s => s.Order);

    foreach (var seeder in seeders)
    {
        await seeder.SeedAsync();
    }
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
