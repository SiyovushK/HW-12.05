using System.Net;
using AutoMapper;
using Domain.DTOs.InstructorDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class InstructorService(DataContext context, IMapper mapper) : IInstructorService
{
    public async Task<Response<GetInstructorDTO>> CreateInstructor(CreateInstructorDTO createInstructor)
    {
        var instructor = mapper.Map<Instructor>(createInstructor);

        await context.Instructors.AddAsync(instructor);
        var result = await context.SaveChangesAsync();

        var getInstructorDto = mapper.Map<GetInstructorDTO>(instructor);

        return result == 0
            ? new Response<GetInstructorDTO>(HttpStatusCode.BadRequest, "Instructor wasn't created")
            : new Response<GetInstructorDTO>(getInstructorDto);
    }

    public async Task<Response<GetInstructorDTO>> UpdateInstructor(int InstructorId, GetInstructorDTO updateInstructor)
    {
        var instructor = await context.Instructors.FindAsync(InstructorId);

        if (instructor == null)
            return new Response<GetInstructorDTO>(HttpStatusCode.NotFound, "Instructor not found");

        instructor.FirstName = updateInstructor.FirstName;
        instructor.LastName = updateInstructor.LastName;
        instructor.Phone = updateInstructor.Phone;

        var result = await context.SaveChangesAsync();
        var getInstructorDto = mapper.Map<GetInstructorDTO>(instructor);

        return result == 0
            ? new Response<GetInstructorDTO>(HttpStatusCode.BadRequest, "Instructor wasn't updated")
            : new Response<GetInstructorDTO>(getInstructorDto);
    }

    public async Task<Response<string>> DeleteInstructor(int InstructorId)
    {
        var instructor = await context.Instructors.FindAsync(InstructorId);

        if (instructor == null)
            return new Response<string>(HttpStatusCode.NotFound, "Instructor not found");

        context.Instructors.Remove(instructor);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Instructor wasn't deleted")
            : new Response<string>("Instructor deleted successfully");
    }

    public async Task<Response<List<GetInstructorDTO>>> GetAllInstructors()
    {
        var instructors = await context.Instructors.ToListAsync();

        if (instructors.Count == 0)
            return new Response<List<GetInstructorDTO>>(HttpStatusCode.NotFound, "No instructors found");

        var getInstructorsDto = mapper.Map<List<GetInstructorDTO>>(instructors);

        return new Response<List<GetInstructorDTO>>(getInstructorsDto);
    }

    public async Task<Response<List<GetInstructorDTO>>> GetAllAsync(InstructorFilter filter)
    {
        var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
        var pageSize = filter.PageSize < 10 ? 10 : filter.PageSize;

        var InstructorQuery = context.Instructors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameFilter = filter.Name.ToLower();
            InstructorQuery = InstructorQuery.Where(i => 
                (i.FirstName + " " + i.LastName).ToLower().Contains(nameFilter));
        }

        if (filter.Phone != null)
        {
            InstructorQuery = InstructorQuery.Where(i => i.Phone.Contains(filter.Phone));
        }

        var totalRecords = await InstructorQuery.CountAsync();

        var Instructor = await InstructorQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var InstructorDtos = mapper.Map<List<GetInstructorDTO>>(Instructor);

        return new PagedResponse<List<GetInstructorDTO>>(InstructorDtos, pageNumber, pageSize, totalRecords);
    }

    //Task5
    public async Task<Response<List<InstructorsWithCourseCount>>> InstructorsAndCourseCount()
    {
        var instructors = await context.Instructors
            .Select(i => new InstructorsWithCourseCount
            {
                InstructorId = i.InstructorId,
                FirstName = i.FirstName,
                LastName = i.LastName,
                CourseCount = i.courseAssignments.Count(ca => ca.InstructorId == i.InstructorId)
            })
            .ToListAsync();

        return new Response<List<InstructorsWithCourseCount>>(instructors);
    } 

}