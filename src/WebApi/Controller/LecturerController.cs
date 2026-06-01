using CoreApp.Application.Authorization;
using CoreApp.Application.Dto.Students;
using CoreApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/lecturer")]
[Authorize(Policy = nameof(AppPolicies.LecturerOrAdmin))]
public class LecturerController(ILecturerService lecturerService) : ControllerBase
{
    /// <summary>Lista studentów zapisanych na kurs danego prowadzącego.</summary>
    [HttpGet("courses/{courseId}/students")]
    [ProducesResponseType(typeof(IEnumerable<StudentSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentsForCourse(Guid courseId)
    {
        var students = await lecturerService.GetStudentsForCourseAsync(courseId);
        return Ok(students);
    }
}
