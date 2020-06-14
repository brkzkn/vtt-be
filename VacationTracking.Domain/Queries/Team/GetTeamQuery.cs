using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Team
{
    public class GetTeamQuery : QueryBase<TeamDto>
    {
        [JsonConstructor]
        public GetTeamQuery(int teamId, int companyId, int userId)
        {
            TeamId = teamId;
            CompanyId = companyId;
            UserId = userId;
        }

        [Required]
        public int TeamId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
