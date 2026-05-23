using CoreApp.Application.Dto.Students;
using CoreApp.Application.Paging;
using CoreApp.Application.Services;
using CoreApp.Application.UnitOfWork;
using CoreApp.Domain.Enums;

namespace Infrastructure.Memory;

public class MemoryStudentService(IUniversityUnitOfWork unitOfWork) : IStudentService
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

    public async Task<StudentDetailDto?> FindStudentByIdAsync(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        return student is null ? null : StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto> CreateStudentAsync(StudentCreateDto dto)
    {
        var student = StudentCreateDto.ToEntity(dto);

        await unitOfWork.Students.AddAsync(student);
        await unitOfWork.SaveChangesAsync();

        return StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto?> UpdateStudentAsync(Guid id, StudentUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null)
        {
            return null;
        }

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.NationalId = dto.NationalId;
        student.Email = dto.Email;
        student.StudentId = dto.StudentId;
        student.ProgramName = dto.ProgramName;
        student.YearOfStudy = dto.YearOfStudy;
        student.Status = dto.Status;

        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return StudentDetailDto.FromEntity(student);
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
