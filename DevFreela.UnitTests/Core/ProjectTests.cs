using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.UnitTests.Helpers;
using FluentAssertions;

namespace DevFreela.UnitTests.Core
{
    public class ProjectTests
    {
        [Fact]
        public void ProjectIsCreated_Start_Success()
        {
            //arrange
            var project = FakeDataHelper.CreateFakeProject();

            //act
            project.Start();

            //Assert
            project.Status.Should().Be(ProjectStatusEnum.InProgress);
            project.StartedAt.Should().NotBeNull();

            //Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            //Assert.NotNull(project.StartedAt);

            Assert.True(project.Status == ProjectStatusEnum.InProgress);
            Assert.False(project.StartedAt is null);
        }

        [Fact]
        public void ProjectIsInInvalidState_Start_ThrowsException()
        {
            //arrange
            var project = FakeDataHelper.CreateFakeProject();
            project.Start();

            //act + assert
            Action? start = project.Start;

            var exception = Assert.Throws<InvalidOperationException>(start);

            Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);

            start.Should().Throw<InvalidOperationException>().WithMessage(Project.INVALID_STATE_MESSAGE);
        }
    }
}
