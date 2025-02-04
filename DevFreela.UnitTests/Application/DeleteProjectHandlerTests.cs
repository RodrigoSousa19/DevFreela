﻿using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using NSubstitute;

namespace DevFreela.UnitTests.Application
{
    public class DeleteProjectHandlerTests
    {
        [Fact]
        public async Task ProjectExists_Delete_Sucess_NSubstitute()
        {
            //Arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(1).Returns(Task.FromResult((Project?)project));
            repository.Update(Arg.Any<Project>()).Returns(Task.CompletedTask);

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(1);
            //Act
            var result = await handler.Handle(command, new CancellationToken());


            //Assert
            Assert.True(result.IsSuccess);
            await repository.Received(1).GetById(1);
            await repository.Received(1).Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectExists_Delete_Sucess_Moq()
        {
            //Arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);

            var repository = Mock.Of<IProjectRepository>(p => p.GetById(It.IsAny<int>()) == Task.FromResult(project) && p.Update(It.IsAny<Project>()) == Task.CompletedTask);

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(1);
            //Act
            var result = await handler.Handle(command, new CancellationToken());


            //Assert
            Assert.True(result.IsSuccess);
            Mock.Get(repository).Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task ProjectDoesNotExist_Delete_Error_NSubstitute()
        {
            //Arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(Arg.Any<int>()).Returns(Task.FromResult((Project?)null));

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(1);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DeleteProjectHandler.PROJECT_NOT_FOUND,result.Message);

            await repository.Received(1).GetById(Arg.Any<int>());
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectDoesNotExist_Delete_Error_Moq()
        {
            //Arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);

            var repository = Mock.Of<IProjectRepository>(p => p.GetById(It.IsAny<int>()) == Task.FromResult((Project?)null) && p.Update(It.IsAny<Project>()) == Task.CompletedTask);

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(1);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DeleteProjectHandler.PROJECT_NOT_FOUND, result.Message);

            Mock.Get(repository).Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>()), Times.Never);
        }

    }
}
