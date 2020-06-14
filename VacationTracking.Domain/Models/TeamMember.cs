using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("TeamMember")]
    public class TeamMember
    {
        [Column("TeamID")]
        public int TeamId { get; set; }

        [Column("UserID")]
        public int UserId { get; set; }

        public bool IsApprover { get; set; }
        public bool IsMember { get; set; }

        public User User { get; set; }

        public Team Team { get; set; }
    }

}
