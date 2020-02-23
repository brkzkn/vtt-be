using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IDbConnection connection)
            : base(connection)
        {
        }

        public async Task<Team> GetAsync(Guid teamId, Guid companyId)
        {
            string sqlQuery = $"SELECT * FROM TEAMS WHERE TEAM_ID = '{teamId}' AND COMPANY_ID = '{companyId}'";
            Team result = await Connection.QueryFirstOrDefaultAsync<Team>(sqlQuery);

            return result;
        }

        public async Task<IEnumerable<Team>> GetListAsync(Guid companyId)
        {
            string sql = "SELECT * FROM TEAMS as t  " +
                "JOIN TEAM_MEMBERS as tm on t.team_id = tm.team_id " +
                "JOIN USERS as u on tm.user_id = u.user_id " +
                $"WHERE t.COMPANY_ID = '{companyId}'";

            Dictionary<Guid, Team> teamsDictionary = new Dictionary<Guid, Team>();

            var result = await Connection.QueryAsync<Team, TeamMember, User, Team>(sql, map: (t, tm, u) =>
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

        public async Task<int> InsertAsync(Team team)
        {
            string insertTeamSql = "INSERT INTO TEAMS(TEAM_ID, COMPANY_ID, TEAM_NAME, CREATED_AT, CREATED_BY)" +
                $" VALUES('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}')";

            var affectedRow = await Connection.ExecuteAsync(insertTeamSql);

            return affectedRow;
        }

        public async Task<int> InsertAsync(IEnumerable<Team> teams)
        {
            var insertTeamSql = new StringBuilder("INSERT INTO TEAMS(TEAM_ID, COMPANY_ID, TEAM_NAME, CREATED_AT, CREATED_BY) VALUES");
            int teamCount = teams.AsList().Count;
            int currentTeam = 0;
            foreach (var team in teams)
            {
                if (currentTeam++ == teamCount)
                    insertTeamSql.Append($"('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}');");
                else
                    insertTeamSql.Append($"('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}'),");
            }

            var affectedRow = await Connection.ExecuteAsync(insertTeamSql.ToString());

            return affectedRow;
        }

        public async Task<int> RemoveAsync(Guid companyId, Guid teamId)
        {
            string deleteTeamSql = $"DELETE FROM TEAMS WHERE TEAM_ID = '{teamId}' and COMPANY_ID = '{companyId}'";
            var affectedRow = await Connection.ExecuteAsync(deleteTeamSql);
            return affectedRow;
        }

        public async Task<int> UpdateAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(Team));

            string query = "UPDATE TEAMS SET " +
                $"TEAM_NAME = '{team.TeamName}', " +
                $"UPDATED_AT = '{team.UpdatedAt}', " +
                $"UPDATED_BY = '{team.UpdatedBy}' " +
                $"WHERE TEAM_ID = '{team.TeamId}' AND COMPANY_ID = '{team.CompanyId}';";

            var affectedRow = await Connection.ExecuteAsync(query);

            return affectedRow;
        }
    }
}
