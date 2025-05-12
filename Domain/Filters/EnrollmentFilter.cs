namespace Domain.Filters;

public class EnrollmentFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int? StudentId { get; set; }
    public int? CourseId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? GradeFrom { get; set; }
    public int? GradeTo { get; set; }
}