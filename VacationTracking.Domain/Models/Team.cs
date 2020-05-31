using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("Team")]
    public class Team : BaseModel
    {
        public Team()
        {
            HolidaysTeam = new HashSet<HolidayTeam>();
            TeamMembers = new HashSet<TeamMember>();
        }

        [Column("TeamID")]
        public int TeamId { get; set; }

        [Column("CompanyID")]
        public int CompanyId { get; set; }

        [StringLength(150)]
        [Required]
        public string TeamName { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<HolidayTeam> HolidaysTeam { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
