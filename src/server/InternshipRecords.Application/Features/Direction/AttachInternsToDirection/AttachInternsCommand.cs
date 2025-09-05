using MediatR;

namespace InternshipRecords.Application.Features.Direction.AttachInternsToDirection;

public record AttachInternsCommand(Guid DirectionId, Guid[] InternIds) : IRequest<Unit>;