using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, Guid>
{
    private readonly IInternRepository _internRepository;

    public UpdateInternCommandHandler(IInternRepository internRepository)
    {
        _internRepository = internRepository;
    }

    public async Task<Guid> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
    {
        var intern = await _internRepository.GetByIdAsync(request.Intern.Id);
        if (intern is null)
            throw new KeyNotFoundException($"Intern with id {request.Intern.Id} not found");

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

        return intern.Id;
    }
}