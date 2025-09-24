using AutoMapper;
using InternshipRecords.Application.Interfaces;
using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public class UpdateDirectionCommandHandler : IRequestHandler<UpdateDirectionCommand, MbResult<DirectionDto>>
{
    private readonly IDirectionRepository _directionRepository;
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public UpdateDirectionCommandHandler(IDirectionRepository directionRepository, IUnitOfWork uow, IMapper mapper,
        IInternRepository internRepository)
    {
        _directionRepository = directionRepository;
        _uow = uow;
        _mapper = mapper;
        _internRepository = internRepository;
    }

    public async Task<MbResult<DirectionDto>> Handle(UpdateDirectionCommand request,
        CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var direction = await _directionRepository.GetByIdAsync(request.Direction.Id);

            if (direction == null) throw new KeyNotFoundException(nameof(direction));

            direction.Name = request.Direction.Name;
            direction.Description = request.Direction.Description;
            direction.UpdatedAt = DateTime.UtcNow;
            await _directionRepository.UpdateAsync(direction);

            var internsToAssign = await _internRepository.GetManyAsync(request.Direction.InternIds!);
            foreach (var intern in internsToAssign) intern.DirectionId = request.Direction.Id;

            var previously = await _internRepository.GetByDirectionIdAsync(request.Direction.Id);
            var toRemove = previously.Where(i => !request.Direction.InternIds!.Contains(i.Id)).ToList();
            foreach (var intern in toRemove) intern.DirectionId = null;

            await _uow.SaveChangesAsync(cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            return MbResult<DirectionDto>.Success(_mapper.Map<DirectionDto>(direction));
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync(cancellationToken);

            return ex switch
            {
                ArgumentNullException => MbResult<DirectionDto>.Fail(new MbError("NotFound", ex.Message)),
                _ => MbResult<DirectionDto>.Fail(new MbError("Неизвестная ошибка", ex.Message))
            };
        }
    }
}