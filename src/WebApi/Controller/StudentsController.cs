using CoreApp.Application.Dto.Students;
using CoreApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/students")]
public class StudentsController(IStudentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await service.FindAllStudentsPaged(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await service.FindStudentByIdAsync(id);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(StudentCreateDto dto)
    {
        var student = await service.CreateStudentAsync(dto);
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(Guid id, StudentUpdateDto dto)
    {
        var student = await service.UpdateStudentAsync(id, dto);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStudentStatus(Guid id, StudentStatusUpdateDto dto)
    {
        var student = await service.ChangeStudentStatusAsync(id, dto.Status);
        return student is null ? NotFound() : Ok(student);
    }
}
