using MediatR;

namespace InternshipRecords.Application.Features.Direction.GetDirections;

public record GetDirectionsQuery(params string[] QueryParams) : IRequest<ICollection<DirectionDto>>;