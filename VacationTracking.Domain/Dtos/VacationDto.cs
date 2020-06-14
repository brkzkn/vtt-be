using Newtonsoft.Json;
using System;
using VacationTracking.Domain.Enums;

namespace VacationTracking.Domain.Dtos
{
    public class VacationDto : BaseDto
    {
        [JsonProperty("vacationId")]
        public int VacationId { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("approverId")]
        public int ApproverId { get; set; }

        [JsonProperty("leaveTypeId")]
        public int LeaveTypeId { get; set; }

        [JsonProperty("vacationStatus")]
        public VacationStatus VacationStatus { get; set; }

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
