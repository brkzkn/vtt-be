using System.Collections.Generic;

namespace VacationTracking.Domain.Commands.Team
{
    public interface ITeamCommand
    {
        int CompanyId { get; set; }
        int UserId { get; set; }
        string Name { get; set; }
        List<int> Members { get; set; }
        List<int> Approvers { get; set; }
    }
}
