using MediatR;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public record UpdateDirectionCommand(UpdateDirectionRequest Direction) : IRequest<Guid>;