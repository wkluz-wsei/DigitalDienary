using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfStudentRepository(UniversityDbContext context)
    : EfGenericRepository<Student>(context.Students), IStudentRepository
{
    public override async Task<Student?> FindByIdAsync(Guid id)
    {
        return await context.Students
            .Include(s => s.Grades)
                .ThenInclude(g => g.Course)
            .Include(s => s.Grades)
                .ThenInclude(g => g.AcademicYear)
            .Include(s => s.Grades)
                .ThenInclude(g => g.Instructor)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetStudentsByStudyYearAsync(int studyYear)
    {
        return await context.Students
            .AsNoTracking()
            .Where(student => student.YearOfStudy == studyYear)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsByStatusAsync(StudentStatus status)
    {
        return await context.Students
            .AsNoTracking()
            .Where(student => student.Status == status)
            .ToListAsync();
    }
}
