using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("companies")]
    public class Company : BaseModel
    {
        public Company()
        {
            Holidays = new HashSet<Holiday>();
        }

        [Key]
        [Column("company_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CompanyId { get; set; }
        
        [Column("company_name")]
        [StringLength(50)]
        public string CompanyName { get; set; }
        
        [Column("address_1")]
        [StringLength(100)]
        public string Address1 { get; set; }
        
        [Column("address_2")]
        [StringLength(100)]
        public string Address2 { get; set; }
        
        [Column("country")]
        [StringLength(100)]
        public string Country { get; set; }
        
        [Column("logo")]
        public string Logo { get; set; }

        public ICollection<Holiday> Holidays { get; set; }
        public ICollection<LeaveType> LeaveTypes{ get; set; }
        public virtual ICollection<Team> Teams { get; set; }

    }
}
