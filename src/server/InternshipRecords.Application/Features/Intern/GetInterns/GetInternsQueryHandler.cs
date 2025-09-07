using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Intern.GetInterns;

public class GetInternsQueryHandler : IRequestHandler<GetInternsQuery, ICollection<InternDto>>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public GetInternsQueryHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<InternDto>> Handle(GetInternsQuery request, CancellationToken cancellationToken)
    {
        var interns = await _internRepository.GetAllAsync(request.DirectionId, request.ProjectId);
        return _mapper.Map<ICollection<InternDto>>(interns);
    }
}