using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public class DeleteDirectionCommandHandler : IRequestHandler<DeleteDirectionCommand, Guid>
{
    private readonly IDirectionRepository _directionRepository;

    public DeleteDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Guid> Handle(DeleteDirectionCommand request, CancellationToken cancellationToken)
    {
        return await _directionRepository.DeleteAsync(request.DirectionId);
    }
}