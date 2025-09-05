using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.AddDirection;

public class AddDirectionCommandHandler : IRequestHandler<AddDirectionCommand, Guid>
{
    private readonly IDirectionRepository _directionRepository;

    public AddDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Guid> Handle(AddDirectionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Direction
        {
            Name = request.Direction.Name,
            Description = request.Direction.Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _directionRepository.CreateAsync(entity);

        return entity.Id;
    }
}