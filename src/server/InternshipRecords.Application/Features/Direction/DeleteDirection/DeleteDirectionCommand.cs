using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public record DeleteDirectionCommand(Guid DirectionId) : IRequest<MbResult<Guid>>;