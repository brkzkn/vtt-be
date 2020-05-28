using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("holiday_team")]
    public class HolidayTeam
    {
        [Column("holiday_id")]
        public Guid HolidayId { get; set; }
        [Column("team_id")]
        public Guid TeamId { get; set; }

        [ForeignKey("HolidayId")]
        [InverseProperty("HolidayTeams")]
        public Holiday Holiday { get; set; }

        [ForeignKey("TeamId")]
        [InverseProperty("Team")]
        public Team Team { get; set; }
    }
}
