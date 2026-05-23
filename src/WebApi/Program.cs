using CoreApp.Application.Repositories;
using CoreApp.Application.Services;
using CoreApp.Application.UnitOfWork;
using Infrastructure.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IStudentRepository, MemoryStudentRepository>();
builder.Services.AddSingleton<ILecturerRepository, MemoryLecturerRepository>();
builder.Services.AddSingleton<IGradeRepository, MemoryGradeRepository>();
builder.Services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();
builder.Services.AddSingleton<IStudentService, MemoryStudentService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
