using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Dtos
{
    public class HolidayDto : BaseDto
    {
        [JsonProperty("holidayId")]
        public Guid HolidayId { get; set; }

        [JsonProperty("companyId")]
        public Guid CompanyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("isFullDay")]
        public bool IsFullDay { get; set; }

        [JsonProperty("teams")]
        public IList<TeamDto> Teams { get; set; }
    }
}
