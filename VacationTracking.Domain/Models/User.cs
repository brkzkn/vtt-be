using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("users")]
    public class User : BaseModel
    {
        public User()
        {
            VacationsApprover = new HashSet<Vacation>();
            VacationsUser = new HashSet<Vacation>();
        }

        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }

        [Column("company_id")]
        public Guid CompanyId { get; set; }

        [Column("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Column("email")]
        [StringLength(200)]
        public string Email { get; set; }

        [Column("employee_since", TypeName = "date")]
        public DateTime EmployeeSince { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; }

        [Column("account_type")]
        [StringLength(50)]
        public string AccountType { get; set; }

        public ICollection<Vacation> VacationsApprover { get; set; }
        public ICollection<Vacation> VacationsUser { get; set; }
    }
}
