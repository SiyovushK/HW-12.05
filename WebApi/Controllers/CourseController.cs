using Domain.DTOs.CourseDTOs;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseService courseService) : ControllerBase
{

    [HttpPost]
    public async Task<Response<GetCourseDTO>> CreateCourse(CreateCourseDTO createCourse)
    {
        return await courseService.CreateCourse(createCourse);
    }

    [HttpPut("{courseId}")]
    public async Task<Response<GetCourseDTO>> UpdateCourse(int courseId, GetCourseDTO updateCourse)
    {
        return await courseService.UpdateCourse(courseId, updateCourse);
    }

    [HttpDelete("{courseId}")]
    public async Task<Response<string>> DeleteCourse(int courseId)
    {
        return await courseService.DeleteCourse(courseId);
    }

    [HttpGet("all")]
    public async Task<Response<List<GetCourseDTO>>> GetAllCourses()
    {
        return await courseService.GetAllCourses();
    }

    [HttpGet("AverageGradeOfCourses")]
    public async Task<Response<List<CourseAverageGradeDTO>>> GetCoursesAverageGrades()
    {
        return await courseService.GetCoursesAverageGrades();
    }

    [HttpGet("StudentsCountInCourses")]
    public async Task<Response<List<CourseWithStudentCount>>> GetStudentsCount()
    {
        return await courseService.GetStudentsCount();
    }

    [HttpGet]
    public async Task<PagedResponse<List<GetCourseDTO>>> GetAllAsync([FromQuery] CourseFilter filter)
    {
        return await courseService.GetAllAsync(filter);
    }
}