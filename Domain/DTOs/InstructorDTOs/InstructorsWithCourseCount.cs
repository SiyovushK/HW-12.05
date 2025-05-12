namespace Domain.DTOs.InstructorDTOs;

public class InstructorsWithCourseCount
{
    public int InstructorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CourseCount { get; set; }
}