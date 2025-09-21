using MediatR;
using Shared.Models;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public record AddDirectionCommand(AddDirectionRequest Direction) : IRequest<MbResult<DirectionDto>>;