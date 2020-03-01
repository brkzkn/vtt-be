using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class LeaveTypeDto : BaseDto
    {
        [JsonProperty("leaveTypeId")]
        public Guid LeaveTypeId { get; set; }

        [JsonProperty("companyId")]
        public Guid CompanyId { get; set; }
        
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty("isHalfDaysActivated")]
        public bool IsHalfDaysActivated { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("isHideLeaveTypeName")]
        public bool IsHideLeaveTypeName { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("isApproverRequired")]
        public bool IsApproverRequired { get; set; }

        [JsonProperty("defaultDaysPerYear")]
        public int DefaultDaysPerYear { get; set; }

        [JsonProperty("isUnlimited")]
        public bool IsUnlimited { get; set; }

        [JsonProperty("isReasonRequired")]
        public bool IsReasonRequired { get; set; }

        [JsonProperty("isAllowNegativeBalance")]
        public bool IsAllowNegativeBalance { get; set; }

        [JsonProperty("colorCode")]
        public string ColorCode { get; set; }
    }
}
