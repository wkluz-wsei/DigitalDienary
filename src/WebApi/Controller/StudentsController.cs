using CoreApp.Application.Dto.Grades;
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

    [HttpPost("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddGrade([FromRoute] Guid studentId, [FromBody] GradeDto dto)
    {
        var grade = await service.AddGrade(studentId, dto);
        return CreatedAtAction(nameof(GetGrades), new { studentId }, grade);
    }

    [HttpGet("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrades([FromRoute] Guid studentId)
    {
        var grades = await service.GetGrades(studentId);
        return Ok(grades);
    }

    [HttpPut("{studentId:guid}/grades/{gradeId:guid}")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade([FromRoute] Guid studentId, [FromRoute] Guid gradeId, [FromBody] GradeUpdateDto dto)
    {
        var grade = await service.UpdateGrade(studentId, gradeId, dto);
        return Ok(grade);
    }
}
