namespace Domain.DTOs.InstructorDTOs;

public class GetInstructorDTO
{
    public int InstructorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}