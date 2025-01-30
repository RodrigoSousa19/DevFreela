using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    internal class InsertProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<Project>>
    {
        private readonly DevFreelaDbContext _context;
        private readonly IMediator _mediator;
        public InsertProjectHandler(DevFreelaDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResultViewModel<Project>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
        {
            var project = request.ToEntity();

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            var projectCreated = new ProjectCreatedNotification(project.Id, project.Title, project.TotalCost);
            await _mediator.Publish(projectCreated);

            return ResultViewModel<Project>.Success(project);
        }
    }
}
