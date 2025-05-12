using Domain.DTOs.CourseDTOs;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface ICourseService
{
    Task<Response<GetCourseDTO>> CreateCourse(CreateCourseDTO createCourse);
    Task<Response<GetCourseDTO>> UpdateCourse(int courseId, GetCourseDTO updateCourse);
    Task<Response<string>> DeleteCourse(int courseId);
    Task<Response<List<GetCourseDTO>>> GetAllCourses();
    Task<PagedResponse<List<GetCourseDTO>>> GetAllAsync(CourseFilter filter);
    Task<Response<List<CourseAverageGradeDTO>>> GetCoursesAverageGrades();
    Task<Response<List<CourseWithStudentCount>>> GetStudentsCount();
}