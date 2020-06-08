using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Team
{
    public class CreateTeamCommand : IRequest<TeamDto>, ITeamCommand
    {
        [JsonConstructor]
        public CreateTeamCommand(int companyId, int userId, string name, List<int> members, List<int> approvers)
        {
            Name = name;
            Members = members;
            Approvers = approvers;
            CompanyId = companyId;
            UserId = userId;
        }

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<int> Members { get; set; }
        public List<int> Approvers { get; set; }
    }
}
