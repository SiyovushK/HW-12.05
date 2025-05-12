using AutoMapper;
using Domain.DTOs.CourseAssignmentDTOs;
using Domain.DTOs.CourseDTOs;
using Domain.DTOs.EnrollmentDTOs;
using Domain.DTOs.InstructorDTOs;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;

namespace Infrastructure.InfProfile;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<Student, GetStudentDTO>();
        CreateMap<CreateStudentDTO, Student>();

        CreateMap<Course, GetCourseDTO>();
        CreateMap<CreateCourseDTO, Course>();

        CreateMap<Instructor, GetInstructorDTO>();
        CreateMap<CreateInstructorDTO, Instructor>();

        CreateMap<Enrollment, GetEnrollmentDTO>();
        CreateMap<CreateEnrollmentDTO, Enrollment>();

        CreateMap<CourseAssignment, GetCourseAssignmentDTO>();
        CreateMap<CreateCourseAssignmentDTO, CourseAssignment>();

        CreateMap<Course, CourseWithStudentCount>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.TotalStudents, opt => opt.MapFrom(src => src.Enrollments.Count));

        CreateMap<Course, CourseAverageGradeDTO>()
            .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom(src => src.Enrollments.Any() ? src.Enrollments.Average(e => e.Grade) : 0));
    }
}