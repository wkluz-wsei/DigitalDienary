using CoreApp.Application.Dto.Students;

namespace CoreApp.Application.Services;

public interface ILecturerService
{
    Task<IEnumerable<StudentSummaryDto>> GetStudentsForCourseAsync(Guid courseId);
}
