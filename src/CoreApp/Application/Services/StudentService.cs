using CoreApp.Application.Dto.Grades;
using CoreApp.Application.Dto.Students;
using CoreApp.Application.Exceptions;
using CoreApp.Application.Paging;
using CoreApp.Application.Security;
using CoreApp.Application.UnitOfWork;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Services;

public class StudentService(
    IUniversityUnitOfWork unitOfWork,
    ICurrentUserContext currentUserContext) : IStudentService
{
    public async Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedSize = size < 1 ? 10 : size;

        var students = await unitOfWork.Students.FindPagedAsync(normalizedPage, normalizedSize);
        var items = students.Items
            .Select(StudentSummaryDto.FromEntity)
            .ToList();

        return new PagedResult<StudentSummaryDto>(
            items,
            students.TotalCount,
            students.Page,
            students.PageSize);
    }

    public async Task<StudentDetailDto?> GetById(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        return student is null ? null : StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto> AddStudent(StudentCreateDto dto)
    {
        var student = StudentCreateDto.ToEntity(dto);

        await unitOfWork.Students.AddAsync(student);
        await unitOfWork.SaveChangesAsync();

        return StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null)
        {
            return null;
        }

        dto.UpdateEntity(student);

        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return StudentSummaryDto.FromEntity(student);
    }

    public async Task<GradeDto> AddGrade(Guid studentId, GradeDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");

        var course = await unitOfWork.Courses.FindByIdAsync(dto.CourseId)
            ?? throw new CourseNotFoundException($"Course with id={dto.CourseId} not found!");

        var lecturer = await unitOfWork.Lecturers.FindByIdAsync(dto.LecturerId)
            ?? throw new LecturerNotFoundException($"Lecturer with id={dto.LecturerId} not found!");

        var academicYear = await unitOfWork.AcademicYears.FindByIdAsync(dto.AcademicYearId)
            ?? throw new AcademicYearNotFoundException($"Academic year with id={dto.AcademicYearId} not found!");

        if (!await unitOfWork.Courses.HasStudentEnrollmentAsync(dto.CourseId, studentId))
            throw new UnauthorizedAccessException("Student is not enrolled in this course.");

        if (!await unitOfWork.Courses.IsTaughtByLecturerIdAsync(dto.CourseId, dto.LecturerId))
            throw new UnauthorizedAccessException("Selected lecturer does not teach this course.");

        if (!IsDeanOfficeOrAdministrator() && lecturer.Email != currentUserContext.UserName)
            throw new UnauthorizedAccessException("Lecturer can add grades only as the current lecturer.");

        await EnsureCanManageCourseGrades(dto.CourseId);

        var grade = new Grade
        {
            Student = student,
            Course = course,
            Instructor = lecturer,
            AcademicYear = academicYear,
            Date = dto.Date,
            GradeType = dto.GradeType,
            GradeValue = GradeValueMapper.ToEnum(dto.GradeValue)
        };

        grade.History.Add(CreateGradeHistory(grade, null, grade.GradeValue, "Added"));

        student.Grades.Add(grade);
        await unitOfWork.Grades.AddAsync(grade);
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return GradeDto.FromEntity(grade);
    }

    public async Task<IEnumerable<GradeDto>> GetGrades(Guid studentId)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");

        return student.Grades
            .Select(GradeDto.FromEntity)
            .ToList();
    }

    public async Task<GradeDto> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");

        var grade = student.Grades.FirstOrDefault(g => g.Id == gradeId)
            ?? throw new GradeNotFoundException($"Grade with id={gradeId} not found for student with id={studentId}!");

        await EnsureCanManageCourseGrades(grade.Course.Id);

        var oldValue = grade.GradeValue;
        dto.UpdateEntity(grade);
        var newValue = grade.GradeValue;

        grade.History.Add(CreateGradeHistory(grade, oldValue, newValue, "Updated"));

        await unitOfWork.Grades.UpdateAsync(grade);
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return GradeDto.FromEntity(grade);
    }

    public async Task<StudentDetailDto?> ChangeStudentStatusAsync(Guid id, StudentStatus status)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null)
        {
            return null;
        }

        student.Status = status;

        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return StudentDetailDto.FromEntity(student);
    }

    private async Task EnsureCanManageCourseGrades(Guid courseId)
    {
        if (IsDeanOfficeOrAdministrator())
            return;

        var userName = currentUserContext.UserName
            ?? throw new UnauthorizedAccessException("Authenticated user email is required.");

        if (!await unitOfWork.Courses.IsTaughtByLecturerEmailAsync(courseId, userName))
            throw new UnauthorizedAccessException("Only the course lecturer can manage grades for this course.");
    }

    private bool IsDeanOfficeOrAdministrator()
    {
        return currentUserContext.IsInRole(UserRole.DeanOfficeStaff.ToString())
            || currentUserContext.IsInRole(UserRole.Administrator.ToString());
    }

    private GradeHistory CreateGradeHistory(Grade grade, GradeValue? oldValue, GradeValue newValue, string actionType)
    {
        return new GradeHistory
        {
            Grade = grade,
            OldValue = oldValue,
            NewValue = newValue,
            ChangedByUserId = currentUserContext.UserId ?? "Unknown",
            ChangedByUserName = currentUserContext.UserName ?? "Unknown",
            ChangedAt = DateTime.UtcNow,
            ActionType = actionType
        };
    }
}
