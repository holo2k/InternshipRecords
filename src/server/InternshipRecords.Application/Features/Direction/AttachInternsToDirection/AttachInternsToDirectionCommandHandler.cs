using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.AttachInternsToDirection;

public class AttachInternsToDirectionCommandHandler : IRequestHandler<AttachInternsToDirectionCommand, Unit>
{
    private readonly IDirectionRepository _directionRepository;

    public AttachInternsToDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Unit> Handle(AttachInternsToDirectionCommand request, CancellationToken cancellationToken)
    {
        await _directionRepository.AttachInternsAsync(request.DirectionId, request.InternIds);
        return Unit.Value;
    }
}