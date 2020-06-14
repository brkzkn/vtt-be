using MediatR;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Team
{
    public class CreateTeamCommand : IRequest<TeamDto>, ITeamCommand
    {
        public CreateTeamCommand(int companyId,
                                 int userId,
                                 string name,
                                 List<int> members,
                                 List<int> approvers)
        {
            Name = name;
            Members = members;
            Approvers = approvers;
            CompanyId = companyId;
            UserId = userId;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public string Name { get; }
        public List<int> Members { get; }
        public List<int> Approvers { get; }
    }
}
