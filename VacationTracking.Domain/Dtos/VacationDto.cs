using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class VacationDto : BaseDto
    {
        [JsonProperty("vacationId")]
        public Guid VacationId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("approverId")]
        public Guid ApproverId { get; set; }

        [JsonProperty("leaveTypeId")]
        public Guid LeaveTypeId { get; set; }

        [JsonProperty("vacationStatus")]
        public string VacationStatus { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("isHalfDay")]
        public bool IsHalfDay { get; set; }

    }
}
