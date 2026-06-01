using CoreApp.Application.Dto.Students;
using CoreApp.Application.Security;
using CoreApp.Application.UnitOfWork;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Services;

public class LecturerService(
    IUniversityUnitOfWork unitOfWork,
    ICurrentUserContext currentUserContext) : ILecturerService
{
    public async Task<IEnumerable<StudentSummaryDto>> GetStudentsAsync()
    {
        var userName = currentUserContext.UserName
            ?? throw new UnauthorizedAccessException("Authenticated user email is required.");

        if (IsDeanOfficeOrAdministrator())
        {
            var students = await unitOfWork.Students.FindAllAsync();
            return students.Select(StudentSummaryDto.FromEntity).ToList();
        }

        var courses = await unitOfWork.Courses.FindByLecturerEmailWithEnrollmentsAsync(userName);
        return courses
            .SelectMany(c => c.Enrollments)
            .GroupBy(s => s.Id)
            .Select(g => StudentSummaryDto.FromEntity(g.First()))
            .ToList();
    }

    public async Task<IEnumerable<StudentSummaryDto>> GetStudentsForCourseAsync(Guid courseId)
    {
        if (!IsDeanOfficeOrAdministrator())
        {
            var userName = currentUserContext.UserName
                ?? throw new UnauthorizedAccessException("Authenticated user email is required.");

            if (!await unitOfWork.Courses.IsTaughtByLecturerEmailAsync(courseId, userName))
                throw new UnauthorizedAccessException("Only the course lecturer can access this student list.");
        }

        var course = await unitOfWork.Courses.FindByIdWithEnrollmentsAsync(courseId);
        if (course is null)
            return Enumerable.Empty<StudentSummaryDto>();

        return course.Enrollments
            .Select(StudentSummaryDto.FromEntity)
            .ToList();
    }

    private bool IsDeanOfficeOrAdministrator()
    {
        return currentUserContext.IsInRole(UserRole.DeanOfficeStaff.ToString())
            || currentUserContext.IsInRole(UserRole.Administrator.ToString());
    }
}
