using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class Team : BaseModel
    {
        public Team()
        {
            TeamMembers = new HashSet<TeamMember>();
            HolidayTeams = new HashSet<HolidayTeam>();
        }

        [Key]
        [Column("team_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TeamId { get; set; }

        [Column("company_id")]
        [ForeignKey("fk_company_id")]
        public Guid CompanyId { get; set; }

        [Column("team_name")]
        [StringLength(50)]
        public string TeamName { get; set; }

        public Company Company { get; set; }

        [InverseProperty("Teams")]
        public ICollection<HolidayTeam> HolidayTeams { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
