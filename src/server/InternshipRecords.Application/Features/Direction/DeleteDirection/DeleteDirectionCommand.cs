using MediatR;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public record DeleteDirectionCommand(Guid DirectionId) : IRequest<Guid>;