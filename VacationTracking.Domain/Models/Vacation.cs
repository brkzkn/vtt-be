using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("vacations")]
    public class Vacation : BaseModel
    {
        [Key]
        [Column("vacation_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid VacationId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("approver_id")]
        public Guid? ApproverId { get; set; }

        [Column("leave_type_id")]
        public Guid LeaveTypeId { get; set; }

        [Column("vacation_status")]
        [Required]
        [StringLength(20)]
        public string VacationStatus { get; set; }

        [Column("start_date", TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column("end_date", TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Column("reason")]
        [StringLength(200)]
        public string Reason { get; set; }

        [Column("note")]
        [StringLength(200)]
        public string Note { get; set; }

        [Column("is_half_day")]
        public bool IsHalfDay { get; set; }

        public User Approver { get; set; }
        public LeaveType LeaveType { get; set; }
        public User User{ get; set; }

    }
}
