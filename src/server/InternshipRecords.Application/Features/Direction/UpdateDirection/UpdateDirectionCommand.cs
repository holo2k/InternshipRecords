using MediatR;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public record UpdateDirectionCommand(UpdateDirectionRequest Direction) : IRequest<DirectionDto>;