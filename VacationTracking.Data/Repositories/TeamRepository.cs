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

        public async Task<int> InsertAsync(Team team)
        {
            throw new NotImplementedException();

            //string insertTeamSql = "INSERT INTO TEAMS(TEAM_ID, COMPANY_ID, TEAM_NAME, CREATED_AT, CREATED_BY)" +
            //    $" VALUES('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}')";

            //var affectedRow = await Connection.ExecuteAsync(insertTeamSql);

            //return affectedRow;
        }

        public async Task<int> InsertAsync(IEnumerable<Team> teams)
        {
            throw new NotImplementedException();
            //var insertTeamSql = new StringBuilder("INSERT INTO TEAMS(TEAM_ID, COMPANY_ID, TEAM_NAME, CREATED_AT, CREATED_BY) VALUES");
            //int teamCount = teams.AsList().Count;
            //int currentCount = 0;
            //foreach (var team in teams)
            //{
            //    if (currentCount++ == teamCount)
            //        insertTeamSql.Append($"('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}');");
            //    else
            //        insertTeamSql.Append($"('{team.TeamId}', '{team.CompanyId}', '{team.TeamName}', '{team.CreatedAt}', '{team.CreatedBy}'),");
            //}

            //var affectedRow = await Connection.ExecuteAsync(insertTeamSql.ToString());

            //return affectedRow;
        }

        public async Task<int> RemoveAsync(Guid companyId, Guid teamId)
        {
            throw new NotImplementedException();
            //string deleteTeamSql = $"DELETE FROM TEAMS WHERE TEAM_ID = '{teamId}' and COMPANY_ID = '{companyId}'";
            //var affectedRow = await Connection.ExecuteAsync(deleteTeamSql);
            //return affectedRow;
        }

        public async Task<int> UpdateAsync(Team team)
        {
            throw new NotImplementedException();
            //if (team == null)
            //    throw new ArgumentNullException(nameof(Team));

            //string query = "UPDATE TEAMS SET " +
            //    $"TEAM_NAME = '{team.TeamName}', " +
            //    $"UPDATED_AT = '{team.ModifiedAt}', " +
            //    $"UPDATED_BY = '{team.ModifiedBy}' " +
            //    $"WHERE TEAM_ID = '{team.TeamId}' AND COMPANY_ID = '{team.CompanyId}';";

            //var affectedRow = await Connection.ExecuteAsync(query);

            //return affectedRow;
        }
    }
}
