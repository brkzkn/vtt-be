using Dapper.FluentMap.Mapping;
using System;

namespace VacationTracking.Domain.Models
{
    public class LeaveType : BaseModel
    {
        public Guid LeaveTypeId { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsHalfDaysActivated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHideLeaveTypeName { get; set; }
        public string TypeName { get; set; }
        public bool IsApproverRequired { get; set; }
        public int DefaultDaysPerYear { get; set; }
        public bool IsUnlimited { get; set; }
        public bool IsReasonRequired { get; set; }
        public bool IsAllowNegativeBalance { get; set; }
        public string ColorCode { get; set; }
    }

    public class LeaveTypesMap : EntityMap<LeaveType>
    {
        public LeaveTypesMap()
        {
            Map(p => p.LeaveTypeId).ToColumn("leave_type_id");
            Map(p => p.CompanyId).ToColumn("company_id");
            Map(p => p.IsDefault).ToColumn("is_default");
            Map(p => p.IsHalfDaysActivated).ToColumn("is_half_days_activated");
            Map(p => p.IsActive).ToColumn("is_active");
            Map(p => p.IsDeleted).ToColumn("is_deleted");
            Map(p => p.IsHideLeaveTypeName).ToColumn("is_hide_leave_type_name");
            Map(p => p.TypeName).ToColumn("type_name");
            Map(p => p.IsApproverRequired).ToColumn("is_approver_required");
            Map(p => p.DefaultDaysPerYear).ToColumn("default_days_per_year");
            Map(p => p.IsUnlimited).ToColumn("is_unlimited");
            Map(p => p.IsReasonRequired).ToColumn("is_reason_required");
            Map(p => p.IsAllowNegativeBalance).ToColumn("is_allow_negative_balance");
            Map(p => p.ColorCode).ToColumn("color_code");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }
}
