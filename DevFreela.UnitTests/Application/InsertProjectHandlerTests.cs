using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;
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

            var command = new InsertProjectCommand
            {
                Title = "Novo projeto",
                Description = "Projeto novo",
                IdClient = 1,
                IdFreeLancer = 2,
                Totalcost = 20000
            };

            var handler = new InsertProjectCommandHandler(repository,mediator);

            //Act
            var result = await handler.Handle(command, new CancellationToken());


            //Assert
            Assert.True(result.IsSuccess);
            await repository.Received(1).Add(Arg.Any<Project>());
        }
    }
}
