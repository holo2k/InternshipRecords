using MediatR;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public record AddDirectionCommand(AddDirectionRequest Direction) : IRequest<Guid>;