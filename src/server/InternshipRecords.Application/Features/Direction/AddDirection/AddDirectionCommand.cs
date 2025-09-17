using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public record AddDirectionCommand(AddDirectionRequest Direction) : IRequest<Guid>;