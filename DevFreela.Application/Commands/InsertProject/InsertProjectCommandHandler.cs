using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    public class InsertProjectCommandHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<Project>>
    {
        private readonly IProjectRepository _repository;
        private readonly IMediator _mediator;
        public InsertProjectCommandHandler(IProjectRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<ResultViewModel<Project>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.Add(request.ToEntity());

            var projectCreated = new ProjectCreatedNotification(project.Id, project.Title, project.TotalCost);
            await _mediator.Publish(projectCreated);

            return ResultViewModel<Project>.Success(project);
        }
    }
}
