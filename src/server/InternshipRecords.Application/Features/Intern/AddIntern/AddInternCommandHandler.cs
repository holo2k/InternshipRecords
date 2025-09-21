using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.AddIntern;

public class AddInternCommandHandler : IRequestHandler<AddInternCommand, MbResult<InternDto>>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;

    public AddInternCommandHandler(IInternRepository internRepository, IMapper mapper)
    {
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<InternDto>> Handle(AddInternCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var intern = _mapper.Map<Domain.Entities.Intern>(request.Intern);

            intern.CreatedAt = DateTime.UtcNow;
            intern.UpdatedAt = DateTime.UtcNow;

            await _internRepository.CreateAsync(intern);

            return MbResult<InternDto>.Success(_mapper.Map<InternDto>(intern));
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => MbResult<InternDto>.Fail(new MbError("ValidationFailure", ex.Message)),
                _ => MbResult<InternDto>.Fail(new MbError("Неизвестная ошибка", ex.Message))
            };
        }
    }
}