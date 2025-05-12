using Domain.DTOs.EnrollmentDTOs;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<GetEnrollmentDTO>> CreateEnrollment(CreateEnrollmentDTO createEnrollment)
    {
        return await enrollmentService.CreateEnrollment(createEnrollment);
    }

    [HttpPut("{enrollmentId}")]
    public async Task<Response<GetEnrollmentDTO>> UpdateEnrollment(int enrollmentId, GetEnrollmentDTO updateEnrollment)
    {
        return await enrollmentService.UpdateEnrollment(enrollmentId, updateEnrollment);
    }

    [HttpDelete("{enrollmentId}")]
    public async Task<Response<string>> DeleteEnrollment(int enrollmentId)
    {
        return await enrollmentService.DeleteEnrollment(enrollmentId);
    }

    [HttpGet("all")]
    public async Task<Response<List<GetEnrollmentDTO>>> GetAllEnrollments()
    {
        return await enrollmentService.GetAllEnrollments();
    }

    [HttpGet]
    public async Task<PagedResponse<List<GetEnrollmentDTO>>> GetAllAsync([FromQuery] EnrollmentFilter filter)
    {
        return await enrollmentService.GetAllAsync(filter);
    }
}
