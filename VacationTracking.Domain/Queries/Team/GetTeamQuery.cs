using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Team
{
    public class GetTeamQuery : QueryBase<TeamDto>
    {
        [JsonConstructor]
        public GetTeamQuery(Guid teamId)
        {
            TeamId = teamId;
        }

        [JsonProperty("id")]
        [Required]
        public Guid TeamId { get; set; }
    }
}
