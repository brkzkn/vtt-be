using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Team
{
    public class UpdateTeamCommand : IRequest<TeamDto>
    {
        [JsonConstructor]
        public UpdateTeamCommand(Guid companyId, Guid userId, Guid teamId, string name, List<Guid> members, List<Guid> approvers)
        {
            Name = name;
            Members = members;
            Approvers = approvers;
            CompanyId = companyId;
            TeamId = teamId;
            UserId = userId;
        }

        public Guid TeamId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<Guid> Members { get; set; }
        public List<Guid> Approvers { get; set; }
    }
}
