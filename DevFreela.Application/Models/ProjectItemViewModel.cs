﻿using DevFreela.Core.Entities;

namespace DevFreela.Application.Models
{
    public class ProjectItemViewModel
    {
        public ProjectItemViewModel(int id, string clientName, string freelancerName, decimal totalCost)
        {
            Id = id;
            ClientName = clientName;
            FreelancerName = freelancerName;
            TotalCost = totalCost;
        }

        public int Id { get; private set; }
        public string ClientName { get; private set; }
        public string FreelancerName { get; private set; }
        public decimal TotalCost { get; private set; }

        public static ProjectItemViewModel FromEntity(Project project) => new(project.Id, project.Client.FullName, project.Freelancer.FullName, project.TotalCost);
    }
}
