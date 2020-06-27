using MediatR;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Team
{
    public class UpdateTeamCommand : IRequest<TeamDto>, ITeamCommand
    {
        public UpdateTeamCommand(int companyId,
                                 int userId,
                                 int teamId,
                                 string name,
                                 List<int> members,
                                 List<int> approvers)
        {
            Name = name;
            Members = members;
            Approvers = approvers;
            CompanyId = companyId;
            TeamId = teamId;
            UserId = userId;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public int TeamId { get; }
        public string Name { get; }
        public List<int> Members { get; }
        public List<int> Approvers { get; }
    }
}
