using AutoMapper;
using InternshipRecords.Infrastructure.Persistence;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models.Direction;

namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public class UpdateDirectionCommandHandler : IRequestHandler<UpdateDirectionCommand, DirectionDto>
{
    private readonly AppDbContext _db;
    private readonly IDirectionRepository _directionRepository;
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public UpdateDirectionCommandHandler(IDirectionRepository directionRepository, AppDbContext db, IMapper mapper,
        IInternRepository internRepository)
    {
        _directionRepository = directionRepository;
        _db = db;
        _mapper = mapper;
        _internRepository = internRepository;
    }

    public async Task<DirectionDto> Handle(UpdateDirectionCommand request, CancellationToken cancellationToken)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var direction = await _directionRepository.GetByIdAsync(request.Direction.Id);

            if (direction == null) throw new ArgumentNullException(nameof(direction));

            direction.Name = request.Direction.Name;
            direction.Description = request.Direction.Description;
            direction.UpdatedAt = DateTime.UtcNow;
            await _directionRepository.UpdateAsync(direction);

            var internsToAssign = await _internRepository.GetManyAsync(request.Direction.InternIds!);
            foreach (var intern in internsToAssign) intern.DirectionId = request.Direction.Id;

            var previously = await _internRepository.GetByProjectIdAsync(request.Direction.Id);
            var toRemove = previously.Where(i => !request.Direction.InternIds!.Contains(i.Id)).ToList();
            foreach (var intern in toRemove) intern.DirectionId = null;

            await _db.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return _mapper.Map<DirectionDto>(direction);
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}