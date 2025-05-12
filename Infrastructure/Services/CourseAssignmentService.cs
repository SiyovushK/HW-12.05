using System.Net;
using AutoMapper;
using Domain.DTOs.CourseAssignmentDTOs;
using Domain.DTOs.CourseDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CourseAssignmentService(DataContext context, IMapper mapper) : ICourseAssignmentService
{
    public async Task<Response<GetCourseAssignmentDTO>> CreateCourseAssignment(CreateCourseAssignmentDTO createCourseAssignment)
    {
        var assignment = mapper.Map<CourseAssignment>(createCourseAssignment);

        await context.CourseAssignments.AddAsync(assignment);
        var result = await context.SaveChangesAsync();

        var getCourseAssignmentDTO = mapper.Map<GetCourseAssignmentDTO>(assignment);

        return result == 0
            ? new Response<GetCourseAssignmentDTO>(HttpStatusCode.BadRequest, "Assignment wasn't created")
            : new Response<GetCourseAssignmentDTO>(getCourseAssignmentDTO);
    }

    public async Task<Response<GetCourseAssignmentDTO>> UpdateCourseAssignment(int CourseAssignmentId, GetCourseAssignmentDTO updateCourseAssignment)
    {
        var assignment = await context.CourseAssignments.FindAsync(CourseAssignmentId);

        if (assignment == null)
            return new Response<GetCourseAssignmentDTO>(HttpStatusCode.NotFound, "Assignment not found");

        assignment.CourseId = updateCourseAssignment.CourseId;
        assignment.InstructorId = updateCourseAssignment.InstructorId;

        var result = await context.SaveChangesAsync();
        var getCourseAssignmentDTO = mapper.Map<GetCourseAssignmentDTO>(assignment);

        return result == 0
            ? new Response<GetCourseAssignmentDTO>(HttpStatusCode.BadRequest, "Assignment wasn't updated")
            : new Response<GetCourseAssignmentDTO>(getCourseAssignmentDTO);
    }

    public async Task<Response<string>> DeleteCourseAssignment(int CourseAssignmentId)
    {
        var assignment = await context.CourseAssignments.FindAsync(CourseAssignmentId);

        if (assignment == null)
            return new Response<string>(HttpStatusCode.NotFound, "Assignment not found");

        context.CourseAssignments.Remove(assignment);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Assignment wasn't deleted")
            : new Response<string>("Assignment deleted successfully");
    }

    public async Task<Response<List<GetCourseAssignmentDTO>>> GetAllCourseAssignments()
    {
        var assignments = await context.CourseAssignments.ToListAsync();

        if (assignments.Count == 0)
            return new Response<List<GetCourseAssignmentDTO>>(HttpStatusCode.NotFound, "No assignments found");

        var getCourseAssignmentsDTO = mapper.Map<List<GetCourseAssignmentDTO>>(assignments);

        return new Response<List<GetCourseAssignmentDTO>>(getCourseAssignmentsDTO);
    }

    public async Task<PagedResponse<List<GetCourseAssignmentDTO>>> GetAllAsync(CourseAssignmentFilter filter)
    {
        var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
        var pageSize = filter.PageSize < 10 ? 10 : filter.PageSize;

        var courseAssignments = context.CourseAssignments.AsQueryable();

        if (filter.CourseId != null)
        {
            courseAssignments = courseAssignments.Where(ca => ca.CourseId == filter.CourseId);
        }

        if (filter.InstructorId != null)
        {
            courseAssignments = courseAssignments.Where(ca => ca.InstructorId == filter.InstructorId);
        }

        var totalRecords = await courseAssignments.CountAsync();

        var pagedData = await courseAssignments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dto = mapper.Map<List<GetCourseAssignmentDTO>>(pagedData);

        return new PagedResponse<List<GetCourseAssignmentDTO>>(dto, pageNumber, pageSize, totalRecords);
    }


}