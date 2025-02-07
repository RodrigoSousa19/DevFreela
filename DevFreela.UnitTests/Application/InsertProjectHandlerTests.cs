using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.UnitTests.Helpers;
using FluentAssertions;
using MediatR;
using Moq;
using NSubstitute;

namespace DevFreela.UnitTests.Application
{
    public class InsertProjectHandlerTests
    {
        [Fact]
        public async Task InputDataAreOk_Insert_Success_NSubstitute()
        {
            //Arrange
            var repository = Substitute.For<IProjectRepository>();
            var mediator = Substitute.For<IMediator>();

            repository.Add(Arg.Any<Project>()).Returns(Task.FromResult(new Project("Novo projeto", "Novo projeto",1,2,1234)));

            var command = FakeDataHelper.CreateFakeInsertProjectcommand();

            var handler = new InsertProjectCommandHandler(repository,mediator);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            result.IsSuccess.Should().BeTrue();

            await repository.Received(1).Add(Arg.Any<Project>());
        }

        [Fact]
        public async Task InputDataAreOk_Insert_Success_Moq()
        {
            //Arrange
            var repository = Mock.Of<IProjectRepository>(r => r.Add(It.IsAny<Project>()) == Task.FromResult(new Project("Novo projeto", "Novo projeto", 1, 2, 1234)));
            var mediator = Mock.Of<IMediator>();

            var command = FakeDataHelper.CreateFakeInsertProjectcommand();

            var handler = new InsertProjectCommandHandler(repository, mediator);

            //Act
            var result = await handler.Handle(command, new CancellationToken());


            //Assert
            Assert.True(result.IsSuccess);

            Mock.Get(repository).Verify(m => m.Add(It.IsAny<Project>()), Times.Once);
        }
    }
}
