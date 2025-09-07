using MediatR;

namespace InternshipRecords.Application.Features.Project.AttachInternsToProject;

public record AttachInternsToProjectCommand(Guid ProjectId, Guid[] InternIds) : IRequest<Unit>;