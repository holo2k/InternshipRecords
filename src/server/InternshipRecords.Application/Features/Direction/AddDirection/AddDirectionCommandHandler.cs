using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public class AddDirectionCommandHandler : IRequestHandler<AddDirectionCommand, Guid>
{
    private readonly IDirectionRepository _directionRepository;
    private readonly IMapper _mapper;

    public AddDirectionCommandHandler(IDirectionRepository directionRepository, IMapper mapper)
    {
        _directionRepository = directionRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddDirectionCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Direction>(request.Direction);

        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await _directionRepository.CreateAsync(entity);

        return entity.Id;
    }
}