using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCourseRepository(UniversityDbContext context)
    : EfGenericRepository<Course>(context.Courses), ICourseRepository
{
    public async Task<Course?> FindByIdWithEnrollmentsAsync(Guid id)
    {
        return await context.Courses
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> FindByLecturerEmailWithEnrollmentsAsync(string email)
    {
        return await context.Courses
            .Include(c => c.Enrollments)
            .Where(c => context.Lecturers
                .Where(l => l.Email == email)
                .SelectMany(l => l.TaughtCourses)
                .Any(taughtCourse => taughtCourse.Id == c.Id))
            .ToListAsync();
    }

    public async Task<bool> HasStudentEnrollmentAsync(Guid courseId, Guid studentId)
    {
        return await context.Courses
            .Where(c => c.Id == courseId)
            .SelectMany(c => c.Enrollments)
            .AnyAsync(s => s.Id == studentId);
    }

    public async Task<bool> IsTaughtByLecturerEmailAsync(Guid courseId, string email)
    {
        return await context.Lecturers
            .Where(l => l.Email == email)
            .SelectMany(l => l.TaughtCourses)
            .AnyAsync(c => c.Id == courseId);
    }

    public async Task<bool> IsTaughtByLecturerIdAsync(Guid courseId, Guid lecturerId)
    {
        return await context.Lecturers
            .Where(l => l.Id == lecturerId)
            .SelectMany(l => l.TaughtCourses)
            .AnyAsync(c => c.Id == courseId);
    }
}
