namespace Domain.Filters;

public class CourseFilter
{
    public string? Title { get; set; }
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}