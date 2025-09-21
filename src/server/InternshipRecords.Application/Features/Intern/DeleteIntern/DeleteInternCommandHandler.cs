using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Intern.DeleteIntern;

public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, MbResult<Guid>>
{
    private readonly IInternRepository _internRepository;

    public DeleteInternCommandHandler(IInternRepository internRepository)
    {
        _internRepository = internRepository;
    }

    public async Task<MbResult<Guid>> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _internRepository.DeleteAsync(request.InternId);
            return MbResult<Guid>.Success(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => MbResult<Guid>.Fail(new MbError("NotFound", ex.Message)),
                _ => MbResult<Guid>.Fail(new MbError("Неизвестное исключение", ex.Message))
            };
        }
    }
}