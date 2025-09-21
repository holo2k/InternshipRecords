using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public class GetInternQueryHandler : IRequestHandler<GetInternQuery, MbResult<InternDto>>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public GetInternQueryHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<InternDto>> Handle(GetInternQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var intern = await _internRepository.GetByIdAsync(request.Id);
            return MbResult<InternDto>.Success(_mapper.Map<InternDto>(intern));
        }
        catch (Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => MbResult<InternDto>.Fail(new MbError("NotFound", ex.Message)),
                _ => MbResult<InternDto>.Fail(new MbError("Неизвестная ошибка", ex.Message))
            };
        }
    }
}