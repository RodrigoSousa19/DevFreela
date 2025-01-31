using DevFreela.Core.Entities;
using DevFreela.Core.Enums;

namespace DevFreela.UnitTests.Core
{
    public class ProjectTests
    {
        [Fact]
        public void ProjectIsCreated_Start_Success()
        {
            //arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);

            //act
            project.Start();

            //Assert
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);

            Assert.True(project.Status == ProjectStatusEnum.InProgress);
            Assert.False(project.StartedAt is null);
        }

        [Fact]
        public void ProjectIsInInvalidState_Start_ThrowsException()
        {
            //arrange
            var project = new Project("Projeto A", "Descrição A", 1, 2, 1234.34M);
            project.Start();

            //act + assert
            Action? start = project.Start;

            var exception = Assert.Throws<InvalidOperationException>(start);

            Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);
        }
    }
}
