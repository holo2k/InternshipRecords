using MediatR;
using Shared.Models;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.GetDirections;

public record GetDirectionsQuery(params string[] QueryParams) : IRequest<MbResult<ICollection<DirectionDto>>>;