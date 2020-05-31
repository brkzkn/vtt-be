using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("Company")]
    public class Company : BaseModel
    {
        public Company()
        {
            CompanySettings = new HashSet<CompanySetting>();
            Holidays = new HashSet<Holiday>();
            LeaveTypes = new HashSet<LeaveType>();
            Teams = new HashSet<Team>();
            Users = new HashSet<User>();
        }

        [Column("CompanyID")]
        public int CompanyId { get; set; }
        [StringLength(100)]
        [Required]
        public string CompanyName { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(500)]
        public string Logo { get; set; }

        public virtual ICollection<CompanySetting> CompanySettings { get; set; }
        public virtual ICollection<Holiday> Holidays { get; set; }
        public virtual ICollection<LeaveType> LeaveTypes { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
