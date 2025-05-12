using System.Net;
using AutoMapper;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;    

public class StudentService(DataContext context, IMapper mapper, IEmailService emailService) : IStudentService
{
    public async Task<Response<GetStudentDTO>> CreateStudent(CreateStudentDTO createStudent)
    {
        var student = mapper.Map<Student>(createStudent);

        await context.Students.AddAsync(student);
        var result = await context.SaveChangesAsync();

        var getStudentDto = mapper.Map<GetStudentDTO>(student);

        return result == 0
            ? new Response<GetStudentDTO>(HttpStatusCode.BadRequest, "Student created")
            : new Response<GetStudentDTO>(getStudentDto);
    }

    public async Task<Response<GetStudentDTO>> UpdateStudent(int studentId, GetStudentDTO updateStudent)
    {
        var student = await context.Students.FindAsync(studentId);
        if (student == null)
            return new Response<GetStudentDTO>(HttpStatusCode.NotFound, "Student is not found");
        
        student.FirstName = updateStudent.FirstName;
        student.LastName = updateStudent.LastName;
        student.BirthDate = updateStudent.BirthDate;

        var result = await context.SaveChangesAsync();

        var getStudentDto = mapper.Map<GetStudentDTO>(student);

        return result == 0
            ? new Response<GetStudentDTO>(HttpStatusCode.BadRequest, "Student not updated")
            : new Response<GetStudentDTO>(getStudentDto);
    }

    public async Task<Response<string>> DeleteStudent(int StudentId)
    {
        var student = await context.Students.FindAsync(StudentId);
        if (student == null)
            return new Response<string>(HttpStatusCode.NotFound, "Student is not found");

        context.Students.Remove(student);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Student wansn't deleted")
            : new Response<string>("Student deleted successfully");
    }

    public async Task<Response<List<GetStudentDTO>>> GetAllStudents()
    {
        var AllStudents = await context.Students
            .ToListAsync();

        if (AllStudents.Count == 0)
            return new Response<List<GetStudentDTO>>(HttpStatusCode.NotFound, "No students found");

        var getStudentsDto = mapper.Map<List<GetStudentDTO>>(AllStudents);
        
        return new Response<List<GetStudentDTO>>(getStudentsDto);
    }

    public async Task<Response<string>> ChangePassword(int studentId, string newPassword)
    {
        var student = await context.Students.FindAsync(studentId);
        if (student is null)
            return new(HttpStatusCode.NotFound, "Student not found");

        var hasher = new PasswordHasher<object>();
        student.PasswordHash = hasher.HashPassword(null!, newPassword);
        await context.SaveChangesAsync();

        await emailService.SendEmailAsync(
            to: student.Email,
            subject: "Password Changed",
            body: $"Hello {student.FirstName}, your password was successfully changed on {DateTime.UtcNow}."
        );

        return new Response<string>("Password updated and email sent");
    }

    //Task3
    public async Task<Response<List<StudentsWithCourseCount>>> GetStudentsCourseCount()
    {
        var students = await context.Students
            .Select(s => new StudentsWithCourseCount
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                CourseCount = s.Enrollments.Count
            })
            .ToListAsync();

        if (students.Count == 0)
        {
            return new Response<List<StudentsWithCourseCount>>(HttpStatusCode.NotFound, "No students found");
        }

        return new Response<List<StudentsWithCourseCount>>(students);
    }

    //Task4
    public async Task<Response<List<GetStudentDTO>>> StudentsWithNoCourses()
    {
        var students = await context.Students
            .Include(s => s.Enrollments)
            .Where(e => e.Enrollments.Count == 0)
            .ToListAsync();
        
        if (students.Count == 0)
        {
            return new Response<List<GetStudentDTO>>(HttpStatusCode.NotFound, "No students found");
        }

        var getStudentsDto = mapper.Map<List<GetStudentDTO>>(students);

        return new Response<List<GetStudentDTO>>(getStudentsDto);
    }

    public async Task<Response<List<GetStudentDTO>>> GetAllAsync(StudentFilter filter)
    {
        var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
        var pageSize = filter.PageSize < 10 ? 10 : filter.PageSize;

        var studentQuery = context.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameFilter = filter.Name.ToLower();
            studentQuery = studentQuery.Where(s => 
                (s.FirstName + " " + s.LastName).ToLower().Contains(nameFilter));
        }

        if (filter.From != null)
        {
            var year = DateTime.UtcNow.Year;
            studentQuery = studentQuery.Where(s => year - s.BirthDate.Year >= filter.From);
        }

        if (filter.To != null)
        {
            var year = DateTime.UtcNow.Year;
            studentQuery = studentQuery.Where(s => year - s.BirthDate.Year <= filter.From);
        }

        var totalRecords = await studentQuery.CountAsync();

        var student = await studentQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var studentDtos = mapper.Map<List<GetStudentDTO>>(student);

        return new PagedResponse<List<GetStudentDTO>>(studentDtos, pageNumber, pageSize, totalRecords);
    }
}