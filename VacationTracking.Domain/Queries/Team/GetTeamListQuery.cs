using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Team
{
    public class GetTeamListQuery : QueryBase<IList<TeamDto>>
    {
        [JsonConstructor]
        public GetTeamListQuery(Guid companyId)
        {
            CompanyId = companyId;
        }

        [JsonProperty("id")]
        [Required]
        public Guid CompanyId { get; set; }
    }
}
