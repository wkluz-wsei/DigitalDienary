using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfAcademicYearRepository(UniversityDbContext context)
    : EfGenericRepository<AcademicYear>(context.AcademicYears), IAcademicYearRepository
{
}
