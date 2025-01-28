using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.InsertProject
{
    public class ValidateInsertProjectCommandBehavior : IPipelineBehavior<InsertProjectCommand, ResultViewModel<Project>>
    {
        private readonly DevFreelaDbContext _context;

        public ValidateInsertProjectCommandBehavior(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<Project>> Handle(InsertProjectCommand request, RequestHandlerDelegate<ResultViewModel<Project>> next, CancellationToken cancellationToken)
        {
            var clientExists = await _context.Users.AnyAsync(u => u.Id == request.IdClient);
            var freelancerExists = await _context.Users.AnyAsync(u => u.Id == request.IdFreeLancer);

            if (!clientExists || !freelancerExists)
                return ResultViewModel<Project>.Error("Cliente ou Freelancer inválidos, tente novamente!");

            return await next();
        }
    }
}
