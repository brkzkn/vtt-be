using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class BaseDto
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("modifiedBy")]
        public int? ModifiedBy { get; set; }
    }
}
