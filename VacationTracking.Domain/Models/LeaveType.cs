using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("leave_types")]
    public class LeaveType : BaseModel
    {
        public LeaveType()
        {
            Vacations = new HashSet<Vacation>();
        }

        [Key]
        [Column("leave_type_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid LeaveTypeId { get; set; }

        [Column("company_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CompanyId { get; set; }

        [Column("is_default")]
        public bool? IsDefault { get; set; }

        [Column("is_half_days_activated")]
        public bool? IsHalfDaysActivated { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [Column("is_hide_leave_type_name")]
        public bool? IsHideLeaveTypeName { get; set; }

        [Column("type_name")]
        [StringLength(100)]
        public string TypeName { get; set; }

        [Column("is_approver_required")]
        public bool? IsApproverRequired { get; set; }

        [Column("default_days_per_year")]
        public int DefaultDaysPerYear { get; set; }

        [Column("is_unlimited")]
        public bool IsUnlimited { get; set; }

        [Column("is_reason_required")]
        public bool? IsReasonRequired { get; set; }

        [Column("is_allow_negative_balance")]
        public bool? IsAllowNegativeBalance { get; set; }

        [Column("color_code")]
        [StringLength(10)]
        public string ColorCode { get; set; }

        [ForeignKey("fk_leavetype_companyid_companies_id")]
        public Company Company { get; set; }

        public ICollection<Vacation> Vacations { get; set; }

    }

}
