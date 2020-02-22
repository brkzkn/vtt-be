using MediatR;
using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Commands.Team
{
    public class DeleteTeamCommand : IRequest<bool>
    {
        [JsonConstructor]
        public DeleteTeamCommand(Guid teamId, Guid companyId)
        {
            TeamId = teamId;
            CompanyId = companyId;
        }

        public Guid TeamId { get; set; }
        public Guid CompanyId { get; set; }

    }
}
