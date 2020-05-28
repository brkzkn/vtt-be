using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("team_member")]
    public class TeamMember
    {
        [Column("team_id")]
        public Guid TeamId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("is_appriver")]
        public bool? IsApprover { get; set; }

        [Column("is_member")]
        public bool? IsMember { get; set; }

        public User User { get; set; }

        public Team Team { get; set; }
    }

}
