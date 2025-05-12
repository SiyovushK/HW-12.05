using Domain.DTOs.CourseAssignmentDTOs;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseAssignmentController(ICourseAssignmentService courseAssignmentService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<GetCourseAssignmentDTO>> CreateCourseAssignment(CreateCourseAssignmentDTO createCourseAssignment)
    {
        return await courseAssignmentService.CreateCourseAssignment(createCourseAssignment);
    }

    [HttpPut("{courseAssignmentId}")]
    public async Task<Response<GetCourseAssignmentDTO>> UpdateCourseAssignment(int courseAssignmentId, GetCourseAssignmentDTO updateCourseAssignment)
    {
        return await courseAssignmentService.UpdateCourseAssignment(courseAssignmentId, updateCourseAssignment);
    }

    [HttpDelete("{courseAssignmentId}")]
    public async Task<Response<string>> DeleteCourseAssignment(int courseAssignmentId)
    {
        return await courseAssignmentService.DeleteCourseAssignment(courseAssignmentId);
    }

    [HttpGet("all")]
    public async Task<Response<List<GetCourseAssignmentDTO>>> GetAllCourseAssignments()
    {
        return await courseAssignmentService.GetAllCourseAssignments();
    }

    [HttpGet]
    public async Task<PagedResponse<List<GetCourseAssignmentDTO>>> GetAllAsync([FromQuery] CourseAssignmentFilter filter)
    {
        return await courseAssignmentService.GetAllAsync(filter);
    }
}
