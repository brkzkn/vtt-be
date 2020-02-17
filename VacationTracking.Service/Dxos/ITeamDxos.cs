using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Dxos
{
    public interface ITeamDxos
    {
        TeamDto MapCustomerDto(Domain.Models.Team team);
    }
}
