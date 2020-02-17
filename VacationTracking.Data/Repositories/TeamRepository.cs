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

        public Task<Team> GetAsync(Guid id)
        {
            
            //IEnumerable<City> result = SqlMapper.Query<City>(DbConnection, "SELECT * FROM CITY");
            //var result = SqlMapper.Query(DbConnection, "SELECT * FROM CITY");
            throw new NotImplementedException();
        }
    }
}
