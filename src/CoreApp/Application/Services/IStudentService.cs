using CoreApp.Application.Dto.Grades;
using CoreApp.Application.Dto.Students;
using CoreApp.Application.Paging;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Services;

public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> GetById(Guid id);
    Task<StudentDetailDto> AddStudent(StudentCreateDto dto);
    Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto);
    Task<GradeDto> AddGrade(Guid studentId, GradeDto dto);
    Task<IEnumerable<GradeDto>> GetGrades(Guid studentId);
    Task<GradeDto> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto);
    Task<StudentDetailDto?> ChangeStudentStatusAsync(Guid id, StudentStatus status);
}
