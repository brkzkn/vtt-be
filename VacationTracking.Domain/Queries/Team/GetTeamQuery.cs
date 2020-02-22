using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Team
{
    public class GetTeamQuery : QueryBase<TeamDto>
    {
        [JsonConstructor]
        public GetTeamQuery(Guid teamId, Guid companyId)
        {
            TeamId = teamId;
            CompanyId = companyId;
        }

        [Required]
        public Guid TeamId { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
    }
}
