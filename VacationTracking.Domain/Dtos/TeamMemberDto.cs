using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class TeamMemberDto
    {
        [JsonProperty("teamId")]
        public Guid TeamId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("isApprover")]
        public bool IsApprover { get; set; }

        [JsonProperty("isMember")]
        public bool IsMember { get; set; }

        [JsonProperty("User")]
        public UserDto User { get; set; }
    }
}
