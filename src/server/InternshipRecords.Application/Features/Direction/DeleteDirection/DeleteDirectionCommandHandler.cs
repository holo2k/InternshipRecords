using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Direction.DeleteDirection;

public class DeleteDirectionCommandHandler : IRequestHandler<DeleteDirectionCommand, MbResult<Guid>>
{
    private readonly IDirectionRepository _directionRepository;

    public DeleteDirectionCommandHandler(IDirectionRepository directionRepository)
    {
        _directionRepository = directionRepository;
    }

    public async Task<MbResult<Guid>> Handle(DeleteDirectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = MbResult<Guid>.Success(await _directionRepository.DeleteAsync(request.DirectionId));
            return result;
        }
        catch (Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => MbResult<Guid>.Fail(new MbError("NotFound", ex.Message)),
                InvalidOperationException => MbResult<Guid>.Fail(new MbError("ObjectHasLinkedEntities", ex.Message)),
                _ => MbResult<Guid>.Fail(new MbError("Неизвестное исключение", ex.Message))
            };
        }
    }
}