using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class Holiday : BaseModel
    {
        [Column("HolidayID")]
        public int HolidayId { get; set; }
        
        [Column("CompanyID")]
        public int CompanyId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        
        public bool IsFullDay { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<HolidayTeam> HolidayTeam { get; set; }
    }
}
