using AutoMapper;
using InternshipRecords.Application.Features.Intern.AddIntern;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using Shared.Models.Intern;

namespace InternshipRecords.Tests;

public class AddInternCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Map_CreateAndReturnId()
    {
        // arrange
        var internDto = new AddInternRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "i@i.com"
        };

        var command = new AddInternCommand { Intern = internDto };

        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<IInternRepository>();

        // mapper produces entity and sets Id when created by repo
        var mappedEntity = new Domain.Intern
        {
            Id = Guid.NewGuid(),
            FirstName = internDto.FirstName,
            LastName = internDto.LastName
        };

        mapperMock.Setup(m => m.Map<Domain.Intern>(internDto)).Returns(mappedEntity);

        repoMock.Setup(r => r.CreateAsync(It.IsAny<Domain.Intern>()))
            .Returns(Task.CompletedTask)
            .Callback<Domain.Intern>(i =>
            {
                /* CreateAsync may set Id internally, assume mapper gave it */
            });

        var handler = new AddInternCommandHandler(repoMock.Object, mapperMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(mappedEntity.Id);
        mapperMock.Verify(m => m.Map<Domain.Intern>(internDto), Times.Once);
        repoMock.Verify(r => r.CreateAsync(It.Is<Domain.Intern>(x => x == mappedEntity)), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ShouldBubbleException()
    {
        var internDto = new AddInternRequest { FirstName = "A" };
        var command = new AddInternCommand { Intern = internDto };

        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<IInternRepository>();

        var mappedEntity = new Domain.Intern { Id = Guid.NewGuid(), FirstName = "A" };
        mapperMock.Setup(m => m.Map<Domain.Intern>(internDto)).Returns(mappedEntity);

        repoMock.Setup(r => r.CreateAsync(It.IsAny<Domain.Intern>()))
            .ThrowsAsync(new InvalidOperationException("DB fail"));

        var handler = new AddInternCommandHandler(repoMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}