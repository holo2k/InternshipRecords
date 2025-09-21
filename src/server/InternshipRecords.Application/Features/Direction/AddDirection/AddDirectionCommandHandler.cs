using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public class AddDirectionCommandHandler : IRequestHandler<AddDirectionCommand, MbResult<DirectionDto>>
{
    private readonly IDirectionRepository _directionRepository;
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;


    public AddDirectionCommandHandler(IDirectionRepository directionRepository, IMapper mapper,
        IInternRepository internRepository)
    {
        _directionRepository = directionRepository;
        _mapper = mapper;
        _internRepository = internRepository;
    }

    public async Task<MbResult<DirectionDto>> Handle(AddDirectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Domain.Entities.Direction>(request.Direction);

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            if (request.Direction.InternIds.Any())
            {
                var interns = await _internRepository.GetManyAsync(request.Direction.InternIds);
                entity.Interns = interns.ToList();
            }

            await _directionRepository.CreateAsync(entity);

            return MbResult<DirectionDto>.Success(_mapper.Map<DirectionDto>(entity));
        }
        catch (Exception ex)
        {
            return MbResult<DirectionDto>.Fail(new MbError("Неизвестное исключение", ex.Message));
        }
    }
}