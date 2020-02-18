using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Dtos
{
    public class TeamDto : BaseDto
    {
        [JsonProperty("teamId")]
        public Guid TeamId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("companyID")]
        public Guid CompanyId { get; set; }

        [JsonProperty("teamMembers")]
        public IList<TeamMemberDto> TeamMembers { get; set; }
    }
}
