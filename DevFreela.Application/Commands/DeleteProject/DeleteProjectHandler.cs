using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultViewModel>
    {
        public const string PROJECT_NOT_FOUND = "Não foi possível encontrar o projeto especificado.";
        private readonly IProjectRepository _repository;

        public DeleteProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetById(request.Id);

            if (project is null)
                return ResultViewModel.Error(PROJECT_NOT_FOUND);

            project.SetAsDeleted();

            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
