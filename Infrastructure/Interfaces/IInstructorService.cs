using Domain.DTOs.InstructorDTOs;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IInstructorService
{
    Task<Response<GetInstructorDTO>> CreateInstructor(CreateInstructorDTO createInstructor);
    Task<Response<GetInstructorDTO>> UpdateInstructor(int InstructorId, GetInstructorDTO updateInstructor);
    Task<Response<string>> DeleteInstructor(int InstructorId);
    Task<Response<List<GetInstructorDTO>>> GetAllInstructors();
    Task<Response<List<GetInstructorDTO>>> GetAllAsync(InstructorFilter filter);
    Task<Response<List<InstructorsWithCourseCount>>> InstructorsAndCourseCount();
}