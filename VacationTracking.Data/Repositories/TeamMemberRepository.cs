using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class TeamMemberRepository : BaseRepository, ITeamMemberRepository
    {
        public TeamMemberRepository(IDbConnection connection)
            : base(connection)
        {
        }

        public Task<TeamMember> GetAsync(Guid teamId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TeamMember>> GetListAsync(Guid teamId)
        {
            throw new NotImplementedException();
            //string sql = "SELECT * FROM TEAM_MEMBERS as tm  " +
            //    "JOIN USERS as u on tm.user_id = u.user_id " +
            //    $"WHERE tm.TEAM_ID = '{teamId}'";

            //Dictionary<Guid, TeamMember> teamMemberDictionary = new Dictionary<Guid, TeamMember>();

            //var result = await Connection.QueryAsync<TeamMember, User, TeamMember>(sql, (team_member, user) =>
            //{
            //    team_member.User = user;
            //    return team_member;
            //}, splitOn: "user_id");

            //return result;
        }

        public async Task<int> InsertAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
            //string sql = "INSERT INTO team_members(team_id, user_id, is_approver, is_member) " +
            //        $"VALUES('{teamMember.TeamId}', '{teamMember.UserId}', {teamMember.IsApprover}, {teamMember.IsMember});";

            //var affectedRow = await Connection.ExecuteAsync(sql);

            //return affectedRow;
        }

        public async Task<int> InsertAsync(IEnumerable<TeamMember> teamMembers)
        {
            throw new NotImplementedException();

            //var sql = new StringBuilder("INSERT INTO team_members(team_id, user_id, is_approver, is_member) VALUES");

            //int teamCount = teamMembers.AsList().Count;
            //int currentTeam = 0;
            //foreach (var teamMember in teamMembers)
            //{
            //    currentTeam += 1;
            //    if (currentTeam == teamCount)
            //        sql.Append($"('{teamMember.TeamId}', '{teamMember.UserId}', {teamMember.IsApprover}, {teamMember.IsMember});");
            //    else
            //        sql.Append($"('{teamMember.TeamId}', '{teamMember.UserId}', {teamMember.IsApprover}, {teamMember.IsMember}),");
            //}

            //var affectedRow = await Connection.ExecuteAsync(sql.ToString());

            //return affectedRow;
        }

        public async Task<int> RemoveAsync(Guid teamId)
        {
            throw new NotImplementedException();

            //string sqlQuery = $"DELETE FROM TEAM_MEMBERS WHERE TEAM_ID = '{teamId}';";
            //var result = await Connection.ExecuteAsync(sqlQuery);

            //return result;
        }

        public Task<int> RemoveAsync(Guid teamId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveNotActiveMemberAsync()
        {
            throw new NotImplementedException();
            //string sql = "DELETE FROM team_members " +
            //    $"WHERE is_member = false and is_approver = false";
            //var affectedRow = Connection.ExecuteAsync(sql);

            //return affectedRow;
        }

        public async Task<int> SetAsNonMemberToOtherTeams(IEnumerable<Guid> userIds)
        {
            throw new NotImplementedException();
            //var userIdList = String.Join(",", userIds.Select(x => $"'{x.ToString()}'").ToList());
            //string sql = $"UPDATE team_members SET is_member = false WHERE user_id in ({userIdList})";
            //var affectedRow = await Connection.ExecuteAsync(sql);

            //return affectedRow;
        }

        public Task<int> UpdateAsync(TeamMember model)
        {
            throw new NotImplementedException();
        }

    }
}
