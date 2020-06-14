using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Team
{
    public class GetTeamListQuery : QueryBase<IList<TeamDto>>
    {
        [JsonConstructor]
        public GetTeamListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        [JsonProperty("id")]
        [Required]
        public int CompanyId { get; set; }
    }
}
