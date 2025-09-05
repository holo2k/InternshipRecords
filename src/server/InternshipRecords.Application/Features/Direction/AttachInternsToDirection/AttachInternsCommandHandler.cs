using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Direction.AttachInternsToDirection;

public class AttachInternsCommandHandler : IRequestHandler<AttachInternsCommand, Unit>
{
    private readonly IDirectionRepository _directionRepository;

    public AttachInternsCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<Unit> Handle(AttachInternsCommand request, CancellationToken cancellationToken)
    {
        await _directionRepository.AttachInternsAsync(request.DirectionId, request.InternIds);
        return Unit.Value;
    }
}