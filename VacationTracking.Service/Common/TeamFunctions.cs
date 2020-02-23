using Dapper;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Models;

namespace VacationTracking.Service.Common
{
    public static class TeamFunctions
    {
        public static List<TeamMember> MergeMemberAndApprover(Guid teamId, IEnumerable<Guid> members, IEnumerable<Guid> approvers)
        {
            Dictionary<Guid, TeamMember> teamMembersDic = new Dictionary<Guid, TeamMember>();

            foreach (var member in members)
            {
                TeamMember teamMember = new TeamMember()
                {
                    IsApprover = false,
                    IsMember = true,
                    TeamId = teamId,
                    UserId = member
                };
                teamMembersDic.Add(member, teamMember);
            }

            foreach (var approver in approvers)
            {
                if (!teamMembersDic.TryGetValue(approver, out TeamMember teamMember))
                {
                    teamMember = new TeamMember()
                    {
                        IsApprover = true,
                        IsMember = false,
                        TeamId = teamId,
                        UserId = approver
                    };
                    teamMembersDic.Add(approver, teamMember);
                }
                else
                {
                    teamMember.IsApprover = true;
                }
            }

            return teamMembersDic.Values.AsList();
        }
    }
}
