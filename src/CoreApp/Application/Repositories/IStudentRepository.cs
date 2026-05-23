using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Repositories;

public interface IStudentRepository : IGenericRepositoryAsync<Student>
{
    Task<IEnumerable<Student>> GetStudentsByStudyYearAsync(int studyYear);
    Task<IEnumerable<Student>> GetStudentsByStatusAsync(StudentStatus status);
}
