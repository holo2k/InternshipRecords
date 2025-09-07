using MediatR;

namespace InternshipRecords.Application.Features.Direction.AttachInternsToDirection;

public record AttachInternsToDirectionCommand(Guid DirectionId, Guid[] InternIds) : IRequest<Unit>;