using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacationTracking.Domain.Enums;

namespace VacationTracking.Domain.Models
{
    [Table("Vacation")]
    public class Vacation : BaseModel
    {
        [Column("VacationID")]
        public int VacationId { get; set; }

        [Column("UserID")]
        public int UserId { get; set; }

        [Column("ApproverID")]
        public int? ApproverId { get; set; }

        [Column("LeaveTypeID")]
        public int LeaveTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public VacationStatus VacationStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        [Column("is_half_day")]
        public bool IsHalfDay { get; set; }

        public User Approver { get; set; }
        public LeaveType LeaveType { get; set; }
        public User User{ get; set; }

    }
}
