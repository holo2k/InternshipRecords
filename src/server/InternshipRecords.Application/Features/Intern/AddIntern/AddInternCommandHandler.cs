using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Intern.AddIntern;

public class AddInternCommandHandler : IRequestHandler<AddInternCommand, Guid>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public AddInternCommandHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddInternCommand request, CancellationToken cancellationToken)
    {
        var intern = _mapper.Map<Domain.Entities.Intern>(request.Intern);

        intern.CreatedAt = DateTime.UtcNow;
        intern.UpdatedAt = DateTime.UtcNow;

        await _internRepository.CreateAsync(intern);

        return intern.Id;
    }
}