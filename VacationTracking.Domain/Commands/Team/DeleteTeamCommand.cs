using MediatR;

namespace VacationTracking.Domain.Commands.Team
{
    public class DeleteTeamCommand : IRequest<bool>
    {
        public DeleteTeamCommand(int teamId, int companyId)
        {
            TeamId = teamId;
            CompanyId = companyId;
        }

        public int TeamId { get; }
        public int CompanyId { get; }
    }
}
