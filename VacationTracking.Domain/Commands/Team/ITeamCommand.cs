using System.Collections.Generic;

namespace VacationTracking.Domain.Commands.Team
{
    public interface ITeamCommand
    {
        int CompanyId { get; }
        int UserId { get; }
        string Name { get; }
        List<int> Members { get; }
        List<int> Approvers { get; }
    }
}
