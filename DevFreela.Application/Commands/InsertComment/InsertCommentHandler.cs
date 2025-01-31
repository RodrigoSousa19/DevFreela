using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.InsertComment
{
    internal class InsertCommentHandler : IRequestHandler<InsertCommentCommand, ResultViewModel>
    {
        private readonly IProjectRepository _repository;

        public InsertCommentHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(InsertCommentCommand request, CancellationToken cancellationToken)
        {
            bool exists = await _repository.Exists(request.IdProject);

            if (!exists)
                return ResultViewModel<ProjectViewModel>.Error("Não foi possível localizar o projeto especificado");

            var projectComment = new ProjectComment(request.Content, request.IdProject, request.IdUser);

            await _repository.AddComment(projectComment);

            return ResultViewModel.Success();
        }
    }
}
