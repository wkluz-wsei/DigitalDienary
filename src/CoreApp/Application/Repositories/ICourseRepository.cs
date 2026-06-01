using CoreApp.Domain.Entities;

namespace CoreApp.Application.Repositories;

public interface ICourseRepository : IGenericRepositoryAsync<Course>
{
    Task<Course?> FindByIdWithEnrollmentsAsync(Guid id);
    Task<IEnumerable<Course>> FindByLecturerEmailWithEnrollmentsAsync(string email);
    Task<bool> HasStudentEnrollmentAsync(Guid courseId, Guid studentId);
    Task<bool> IsTaughtByLecturerEmailAsync(Guid courseId, string email);
    Task<bool> IsTaughtByLecturerIdAsync(Guid courseId, Guid lecturerId);
}
