using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Teams> GetAsync(Guid id)
        {
            Teams result = await SqlMapper.QuerySingleOrDefaultAsync<Teams>(DbConnection, $"SELECT * FROM TEAMS WHERE TEAM_ID = '{id}'");

            return result;
        }
    }
}
