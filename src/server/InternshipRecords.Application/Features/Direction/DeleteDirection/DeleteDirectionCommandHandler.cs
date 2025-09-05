using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public class DeleteDirectionCommandHandler : IRequestHandler<DeleteDirectionCommand, Unit>
{
    private readonly IDirectionRepository _directionRepository;

    public DeleteDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Unit> Handle(DeleteDirectionCommand request, CancellationToken cancellationToken)
    {
        await _directionRepository.DeleteAsync(request.DirectionId);
        return Unit.Value;
    }
}