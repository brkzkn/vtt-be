using Dapper.FluentMap.Mapping;
using System;

namespace VacationTracking.Domain.Models
{
    public class TeamMember
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public bool IsApprover { get; set; }
        public bool IsMember { get; set; }

        public User User { get; set; }
    }

    public class TeamMemberMap : EntityMap<TeamMember>
    {
        public TeamMemberMap()
        {
            Map(p => p.TeamId).ToColumn("team_id");
            Map(p => p.UserId).ToColumn("user_id");
            Map(p => p.IsApprover).ToColumn("is_approver");
            Map(p => p.IsMember).ToColumn("is_member");
        }
    }
}
