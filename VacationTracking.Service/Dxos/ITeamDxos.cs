using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Dxos
{
    public interface ITeamDxos
    {
        TeamDto MapTeamDto(Domain.Models.Team team);
        TeamMemberDto MapTeamDto(Domain.Models.TeamMember teamMember);
    }
}
