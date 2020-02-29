using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Models
{
    public class Team : BaseModel
    {
        public Guid TeamId { get; set; }
        public Guid CompanyId { get; set; }
        public string TeamName { get; set; }

        public List<TeamMember> TeamMembers { get; set; }
        public IList<Holiday> Holidays { get; set; }
    }

    public class TeamsMap : EntityMap<Team>
    {
        public TeamsMap()
        {
            Map(p => p.TeamId).ToColumn("team_id");
            Map(p => p.CompanyId).ToColumn("company_id");
            Map(p => p.TeamName).ToColumn("team_name");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }

}
