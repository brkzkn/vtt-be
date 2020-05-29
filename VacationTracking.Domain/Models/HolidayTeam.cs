using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("holiday_team")]
    public class HolidayTeam
    {
        public Guid HolidayId { get; set; }

        [Column("team_id")]
        public Guid TeamId { get; set; }

        public Holiday Holiday { get; set; }

        public Team Team { get; set; }
    }
}
