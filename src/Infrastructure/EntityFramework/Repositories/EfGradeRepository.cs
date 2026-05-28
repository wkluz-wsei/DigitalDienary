using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGradeRepository(UniversityDbContext context)
    : EfGenericRepository<Grade>(context.Grades), IGradeRepository
{
    public override async Task<Grade?> FindByIdAsync(Guid id)
    {
        return await context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Include(g => g.AcademicYear)
            .Include(g => g.Instructor)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
}
