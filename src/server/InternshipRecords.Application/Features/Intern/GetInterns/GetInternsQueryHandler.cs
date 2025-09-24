using AutoMapper;
using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.GetInterns;

public class GetInternsQueryHandler : IRequestHandler<GetInternsQuery, MbResult<ICollection<InternDto>>>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public GetInternsQueryHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<ICollection<InternDto>>> Handle(GetInternsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var interns = await _internRepository.GetAllAsync(request.DirectionId, request.ProjectId);
            var result = _mapper.Map<ICollection<InternDto>>(interns);
            return MbResult<ICollection<InternDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return MbResult<ICollection<InternDto>>.Fail(new MbError("Неизвестное исключение", ex.Message));
        }
    }
}