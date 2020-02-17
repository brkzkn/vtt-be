using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class CompanyDto : BaseDto
    {
        [JsonProperty("companyId")]
        public Guid CompanyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("address_1")]
        public string Address_1 { get; set; }

        [JsonProperty("address_2")]
        public string Address_2 { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}
