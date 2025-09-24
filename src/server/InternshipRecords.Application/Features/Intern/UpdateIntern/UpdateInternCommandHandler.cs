using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, MbResult<Guid>>
{
    private readonly IInternRepository _internRepository;

    public UpdateInternCommandHandler(IInternRepository internRepository)
    {
        _internRepository = internRepository;
    }

    public async Task<MbResult<Guid>> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var intern = await _internRepository.GetByIdAsync(request.Intern.Id);

            intern.UpdatedAt = DateTime.UtcNow;
            intern.BirthDate = request.Intern.BirthDate;
            intern.Email = request.Intern.Email;
            intern.FirstName = request.Intern.FirstName;
            intern.LastName = request.Intern.LastName;
            intern.Gender = request.Intern.Gender;
            intern.Phone = request.Intern.Phone;
            intern.ProjectId = request.Intern.ProjectId;
            intern.DirectionId = request.Intern.DirectionId;

            await _internRepository.UpdateAsync(intern);

            return MbResult<Guid>.Success(intern.Id);
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