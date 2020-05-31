using Newtonsoft.Json;
using System.Collections.Generic;

namespace VacationTracking.Domain.Dtos
{
    public class TeamDto : BaseDto
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("name")]
        public string TeamName { get; set; }

        [JsonProperty("companyID")]
        public int CompanyId { get; set; }

        [JsonProperty("teamMembers")]
        public IEnumerable<TeamMemberDto> TeamMembers { get; set; }
    }
}
