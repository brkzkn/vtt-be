using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacationTracking.Domain.Queries.Holiday;

namespace VacationTracking.Domain.Models
{
    [Table("holidays")]
    public class Holiday : BaseModel
    {
        [Key]
        [Column("holiday_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid HolidayId { get; set; }
        
        [Column("company_id")]
        public Guid CompanyId { get; set; }
        
        [Column("name")]
        [StringLength(100)]
        public string HolidayName { get; set; }
        
        [Column("start_date", TypeName = "date")]
        public DateTime StartDate { get; set; }
        
        [Column("end_date", TypeName = "date")]
        public DateTime? EndDate { get; set; }
        
        [Column("is_full_day")]
        public bool? IsFullDay { get; set; }

        public ICollection<Team> Teams { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("Holidays")]
        public Company Company { get; set; }

        [InverseProperty("HolidayTeams")]
        public ICollection<HolidayTeam> HolidayTeams { get; set; }
    }
}
