using CoreApp.Application.Dto.Students;
using CoreApp.Application.Security;
using CoreApp.Application.UnitOfWork;

namespace CoreApp.Application.Services;

public class LecturerService(
    IUniversityUnitOfWork unitOfWork,
    ICurrentUserContext currentUserContext) : ILecturerService
{
    public async Task<IEnumerable<StudentSummaryDto>> GetStudentsForCourseAsync(Guid courseId)
    {
        var userId = currentUserContext.UserId;
        if (userId == null) throw new UnauthorizedAccessException();
        
        var lecturer = await unitOfWork.Lecturers.FindAllAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(l => l.Email == currentUserContext.UserName));

        if (lecturer == null && !currentUserContext.IsInRole(UserRole.Administrator.ToString()) && !currentUserContext.IsInRole(UserRole.DeanOfficeStaff.ToString()))
        {
             throw new UnauthorizedAccessException("Only lecturers or staff can access student lists.");
        }

        var course = await unitOfWork.Courses.FindByIdAsync(courseId);
        if (course == null) return Enumerable.Empty<StudentSummaryDto>();


        return course.Enrollments.Select(StudentSummaryDto.FromEntity);
    }
}
