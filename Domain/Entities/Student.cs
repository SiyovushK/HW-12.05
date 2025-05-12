using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Student
{
    public int StudentId { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public List<Enrollment> Enrollments { get; set; } = new();
}