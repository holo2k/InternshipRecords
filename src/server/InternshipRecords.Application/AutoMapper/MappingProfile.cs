using AutoMapper;
using InternshipRecords.Domain.Entities;
using Shared.Models.Direction;
using Shared.Models.Intern;
using Shared.Models.Project;

namespace InternshipRecords.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Direction, DirectionDto>()
            .ForMember(dest => dest.Interns, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Interns, opt => opt.Ignore());

        CreateMap<Intern, InternDto>()
            .ForMember(dest => dest.Direction, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Direction, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore());

        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Interns, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Interns, opt => opt.Ignore());

        CreateMap<AddInternRequest, Intern>().ReverseMap();
        CreateMap<AddDirectionRequest, Direction>().ReverseMap();
        CreateMap<AddProjectRequest, Project>().ReverseMap();
    }
}