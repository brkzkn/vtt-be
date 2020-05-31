using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("LeaveType")]
    public class LeaveType : BaseModel
    {
        public LeaveType()
        {
            Vacations = new HashSet<Vacation>();
        }

        [Column("LeaveTypeID")]
        public int LeaveTypeId { get; set; }
        [Column("CompanyID")]
        public int CompanyId { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsHalfDaysActivated { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHideLeaveTypeName { get; set; }
        [StringLength(100)]
        public string LeaveTypeName { get; set; }
        public bool? IsApproverRequired { get; set; }
        public int DefaultDaysPerYear { get; set; }
        public bool IsUnlimited { get; set; }
        public bool? IsReasonRequired { get; set; }
        public bool? IsAllowNegativeBalance { get; set; }
        [StringLength(15)]
        public string ColorCode { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }

    }

}
