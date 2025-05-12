namespace Domain.Filters;

public class CourseAssignmentFilter
{
    public int? CourseId { get; set; }
    public int? InstructorId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}