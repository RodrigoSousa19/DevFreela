using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    internal class InsertProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<Project>>
    {
        private readonly DevFreelaDbContext _context;
        public InsertProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<Project>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
        {
            var project = request.ToEntity();

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return ResultViewModel<Project>.Success(project);
        }
    }
}
