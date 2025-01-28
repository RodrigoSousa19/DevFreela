using DevFreela.Core.Entities;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    public class InsertProjectCommand : IRequest<ResultViewModel<Project>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int IdClient { get; set; }
        public int IdFreeLancer { get; set; }
        public decimal Totalcost { get; set; }

        public Project ToEntity() => new(Title, Description, IdClient, IdFreeLancer, Totalcost);
    }
}
