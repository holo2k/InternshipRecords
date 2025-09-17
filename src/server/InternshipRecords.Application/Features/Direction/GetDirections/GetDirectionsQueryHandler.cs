using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.GetDirections;

public class GetDirectionsQueryHandler : IRequestHandler<GetDirectionsQuery, ICollection<DirectionDto>>
{
    private readonly IDirectionRepository _directionRepository;
    private readonly IMapper _mapper;

    public GetDirectionsQueryHandler(IDirectionRepository directionRepository, IMapper mapper)
    {
        _directionRepository = directionRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<DirectionDto>> Handle(GetDirectionsQuery request, CancellationToken cancellationToken)
    {
        var directions = await _directionRepository.GetAllAsync(request.QueryParams);
        return _mapper.Map<ICollection<DirectionDto>>(directions);
    }
}