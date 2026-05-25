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
    public async Task<IActionResult> GetStudent(Guid id)
    {
        var dto = await service.GetById(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var result = await service.AddStudent(dto);
        return CreatedAtAction(nameof(GetStudent), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(Guid id, StudentUpdateDto dto)
    {
        var student = await service.UpdateStudent(id, dto);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStudentStatus(Guid id, StudentStatusUpdateDto dto)
    {
        var student = await service.ChangeStudentStatusAsync(id, dto.Status);
        return student is null ? NotFound() : Ok(student);
    }
}
