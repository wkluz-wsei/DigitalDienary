using CoreApp.Application.Dto.Students;
using CoreApp.Application.Paging;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Services;

public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> FindStudentByIdAsync(Guid id);
    Task<StudentDetailDto> CreateStudentAsync(StudentCreateDto dto);
    Task<StudentDetailDto?> UpdateStudentAsync(Guid id, StudentUpdateDto dto);
    Task<StudentDetailDto?> ChangeStudentStatusAsync(Guid id, StudentStatus status);
}
