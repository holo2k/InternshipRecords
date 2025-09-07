using AutoMapper;
using InternshipRecords.Application.Features.Direction;
using InternshipRecords.Application.Features.Direction.AddDirection;
using InternshipRecords.Application.Features.Intern;
using InternshipRecords.Application.Features.Intern.AddIntern;
using InternshipRecords.Application.Features.Project;
using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Direction, DirectionDto>().ReverseMap();
        CreateMap<Intern, InternDto>().ReverseMap();
        CreateMap<Project, ProjectDto>().ReverseMap();

        CreateMap<AddInternRequest, Intern>();
        CreateMap<AddDirectionRequest, Direction>();
    }
}