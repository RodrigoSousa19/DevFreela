using DevFreela.API.Entities;

namespace DevFreela.API.Models
{
    public class CreateProjectInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int IdClient { get; set; }
        public int IdFreeLancer { get; set; }
        public Decimal Totalcost { get; set; }

        public Project ToEntity() => new(Title, Description, IdClient, IdFreeLancer, Totalcost);
    }
}
