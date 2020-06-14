using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("HolidayTeam")]
    public class HolidayTeam
    {
        [Column("HolidayID")]
        public int HolidayId { get; set; }
        [Column("TeamID")]
        public int TeamId { get; set; }

        public Holiday Holiday { get; set; }
        public Team Team { get; set; }
    }
}
