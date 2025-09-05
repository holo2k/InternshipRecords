using MediatR;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public record UpdateDirectionCommand(DirectionDto Direction) : IRequest<Guid>;