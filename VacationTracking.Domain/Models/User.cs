using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacationTracking.Domain.Enums;

namespace VacationTracking.Domain.Models
{

    [Table("User")]
    public class User : BaseModel
    {
        public User()
        {
            TeamMembers = new HashSet<TeamMember>();
            UserSettings = new HashSet<UserSetting>();
            VacationApprovers = new HashSet<Vacation>();
            Vacations = new HashSet<Vacation>();
        }

        [Column("UserID")]
        public int UserId { get; set; }

        [Column("CompanyID")]
        public int CompanyId { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmployeeSince { get; set; }

        [StringLength(50)]
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public UserStatus Status { get; set; }

        [StringLength(50)]
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public AccountType AccountType { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public virtual ICollection<Vacation> VacationApprovers { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}
