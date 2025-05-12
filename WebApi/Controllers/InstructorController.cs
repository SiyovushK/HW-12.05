using Domain.DTOs.InstructorDTOs;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorController(IInstructorService instructorService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<GetInstructorDTO>> CreateInstructor(CreateInstructorDTO createInstructor)
    {
        return await instructorService.CreateInstructor(createInstructor);
    }

    [HttpPut("{instructorId}")]
    public async Task<Response<GetInstructorDTO>> UpdateInstructor(int instructorId, GetInstructorDTO updateInstructor)
    {
        return await instructorService.UpdateInstructor(instructorId, updateInstructor);
    }

    [HttpDelete("{instructorId}")]
    public async Task<Response<string>> DeleteInstructor(int instructorId)
    {
        return await instructorService.DeleteInstructor(instructorId);
    }

    [HttpGet("all")]
    public async Task<Response<List<GetInstructorDTO>>> GetAllInstructors()
    {
        return await instructorService.GetAllInstructors();
    }

    [HttpGet]
    public async Task<Response<List<GetInstructorDTO>>> GetAllAsync([FromQuery] InstructorFilter filter)
    {
        return await instructorService.GetAllAsync(filter);
    }

    [HttpGet("InstructorsWithCouseCount")]
    public async Task<Response<List<InstructorsWithCourseCount>>> InstructorsAndCourseCount()
    {
        return await instructorService.InstructorsAndCourseCount();
    }
}
