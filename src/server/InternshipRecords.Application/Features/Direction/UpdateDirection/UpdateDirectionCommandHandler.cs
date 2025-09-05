using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public class UpdateDirectionCommandHandler : IRequestHandler<UpdateDirectionCommand, Guid>
{
    private readonly IDirectionRepository _directionRepository;

    public UpdateDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Guid> Handle(UpdateDirectionCommand request, CancellationToken cancellationToken)
    {
        var direction = await _directionRepository.GetByIdAsync(request.Direction.Id);
        if (direction is null)
            throw new KeyNotFoundException($"Direction with id {request.Direction.Id} not found");

        direction.Name = request.Direction.Name;
        direction.Description = request.Direction.Description;
        direction.UpdatedAt = DateTime.UtcNow;

        await _directionRepository.SaveAsync();

        return direction.Id;
    }
}