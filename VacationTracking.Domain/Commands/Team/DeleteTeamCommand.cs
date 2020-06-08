using MediatR;
using Newtonsoft.Json;

namespace VacationTracking.Domain.Commands.Team
{
    public class DeleteTeamCommand : IRequest<bool>
    {
        [JsonConstructor]
        public DeleteTeamCommand(int teamId, int companyId)
        {
            TeamId = teamId;
            CompanyId = companyId;
        }

        public int TeamId { get; set; }
        public int CompanyId { get; set; }
    }
}
