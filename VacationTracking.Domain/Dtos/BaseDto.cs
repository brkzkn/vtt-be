using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class BaseDto
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public Guid UpdatedBy { get; set; }
    }
}
