using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public class GetInternQueryHandler : IRequestHandler<GetInternQuery, InternDto>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public GetInternQueryHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<InternDto> Handle(GetInternQuery request, CancellationToken cancellationToken)
    {
        var intern = await _internRepository.GetByIdAsync(request.Id) ??
                     throw new InvalidOperationException("Такого интерна не существует");
        return _mapper.Map<InternDto>(intern);
    }
}