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

        public async Task<Team> GetAsync(Guid teamId)
        {
            Team result = await SqlMapper.QuerySingleOrDefaultAsync<Team>(DbConnection, $"SELECT * FROM TEAMS WHERE TEAM_ID = '{teamId}'");

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
    }
}
