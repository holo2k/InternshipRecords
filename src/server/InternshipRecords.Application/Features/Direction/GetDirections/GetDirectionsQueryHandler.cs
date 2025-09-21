using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.GetDirections;

public class GetDirectionsQueryHandler : IRequestHandler<GetDirectionsQuery, MbResult<ICollection<DirectionDto>>>
{
    private readonly IDirectionRepository _directionRepository;
    private readonly IMapper _mapper;

    public GetDirectionsQueryHandler(IDirectionRepository directionRepository, IMapper mapper)
    {
        _directionRepository = directionRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<ICollection<DirectionDto>>> Handle(GetDirectionsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var directions = await _directionRepository.GetAllAsync(request.QueryParams);
            var result = _mapper.Map<ICollection<DirectionDto>>(directions);
            return MbResult<ICollection<DirectionDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return MbResult<ICollection<DirectionDto>>.Fail(new MbError("Неизвестная ошибка", ex.Message));
        }
    }
}