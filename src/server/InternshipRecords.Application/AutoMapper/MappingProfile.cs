using AutoMapper;
using InternshipRecords.Domain.Entities;
using Shared.Models;
using Shared.Models.Direction;
using Shared.Models.Intern;
using Shared.Models.Project;

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
        CreateMap<AddProjectRequest, Project>();
    }
}