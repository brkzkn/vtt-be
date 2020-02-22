using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Team> GetAsync(Guid teamId, Guid companyId)
        {
            string sqlQuery = $"SELECT * FROM TEAMS WHERE TEAM_ID = '{teamId}' AND COMPANY_ID = '{companyId}'";
            Team result = await SqlMapper.QuerySingleOrDefaultAsync<Team>(DbConnection, sqlQuery);

            return result;
        }

        public async Task<IList<Team>> GetListAsync(Guid companyId)
        {
            string sql = "SELECT * FROM TEAMS as t  " +
                "JOIN TEAM_MEMBERS as tm on t.team_id = tm.team_id " +
                "JOIN USERS as u on tm.user_id = u.user_id " +
                $"WHERE t.COMPANY_ID = '{companyId}'";

            Dictionary<Guid, Team> teamsDictionary = new Dictionary<Guid, Team>();

            var result = await SqlMapper.QueryAsync<Team, TeamMember, User, Team>(DbConnection, sql, map: (t, tm, u) =>
            {
                tm.User = u;
                if (!teamsDictionary.TryGetValue(t.TeamId, out Team teamEntry))
                {
                    teamEntry = t;
                    teamEntry.TeamMembers = new List<TeamMember>();
                    teamsDictionary.Add(t.TeamId, teamEntry);
                }
                teamEntry.TeamMembers.Add(tm);
                return teamEntry;

            }, splitOn: "team_id, user_id");

            return teamsDictionary.Values.AsList();
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            /*
             1. Delete team_members where CompanyId = X and TeamId = Y and IsMember = true and user_id in (request.Members)
             2. Merge two list (request.Member and request.Approver)
                -   new TeamMember(teamId, Member[index]/Approver[index),  Member.IsExist(), Approver.IsExist())
             3. Insert team table
             */
            DbConnection.Open();
            using (var transaction = DbConnection.BeginTransaction())
            {
                string deleteSql = "DELETE FROM TEAM_MEMBERS " +
                    $"WHERE TEAM_ID = '{team.TeamId}'";
                var affectedRow = await SqlMapper.ExecuteAsync(DbConnection, deleteSql);

                string insertTeamSql = "INSERT INTO TEAMS(TEAM_ID, COMPANY_ID, TEAM_NAME, CREATED_AT, CREATED_BY)" +
                    $" VALUES('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}')";

                affectedRow = await SqlMapper.ExecuteAsync(DbConnection, insertTeamSql);

                foreach (var teamMember in team.TeamMembers)
                {
                    string insertTeamMemberSql = "INSERT INTO TEAM_MEMBERS(TEAM_ID, USER_ID, IS_APPROVER, IS_MEMBER) " +
                        $"VALUES('{teamMember.TeamId}', '{teamMember.UserId}', '{teamMember.IsApprover}', '{teamMember.IsMember}')";
                    affectedRow = await SqlMapper.ExecuteAsync(DbConnection, insertTeamMemberSql);
                }
                await transaction.CommitAsync();
            }
            DbConnection.Close();

            return team;
        }

        public async Task<bool> DeleteTeamAsync(Guid companyId, Guid teamId)
        {
            /*
             * 1. Delete team_members
             * 2. Delete holiday_team
             * 2. Delete teams
             */
            DbConnection.Open();
            using (var transaction = DbConnection.BeginTransaction())
            {
                string deleteTeamMemberSql = $"DELETE FROM TEAM_MEMBERS WHERE TEAM_ID = '{teamId}'";
                var affectedRow = await SqlMapper.ExecuteAsync(DbConnection, deleteTeamMemberSql);

                string deleteHolidayTeamSql = $"DELETE FROM HOLIDAY_TEAM WHERE TEAM_ID = '{teamId}'";
                affectedRow = await SqlMapper.ExecuteAsync(DbConnection, deleteHolidayTeamSql);

                string deleteTeamSql = $"DELETE FROM TEAMS WHERE TEAM_ID = '{teamId}'";
                affectedRow = await SqlMapper.ExecuteAsync(DbConnection, deleteTeamSql);

                transaction.Commit();
            }
            DbConnection.Close();
            return true;
        }
    }
}
