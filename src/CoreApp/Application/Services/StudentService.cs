using CoreApp.Application.Dto.Grades;
using CoreApp.Application.Dto.Students;
using CoreApp.Application.Exceptions;
using CoreApp.Application.Paging;
using CoreApp.Application.UnitOfWork;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Services;

public class StudentService(IUniversityUnitOfWork unitOfWork) : IStudentService
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

        dto.UpdateEntity(grade);

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
}
