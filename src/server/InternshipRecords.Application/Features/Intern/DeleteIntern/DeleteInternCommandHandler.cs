using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Intern.DeleteIntern;

public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, Unit>
{
    private readonly IInternRepository _internRepository;

    public DeleteInternCommandHandler(IInternRepository internRepository)
    {
        _internRepository = internRepository;
    }

    public async Task<Unit> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {
        await _internRepository.DeleteAsync(request.InternId);
        return Unit.Value;
    }
}