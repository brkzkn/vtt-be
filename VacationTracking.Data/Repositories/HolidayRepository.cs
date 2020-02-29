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
    public class HolidayRepository : BaseRepository, IHolidayRepository
    {
        public HolidayRepository(IDbConnection connection)
            : base(connection)
        {
        }

        public Task<Holiday> GetAsync(Guid companyId, Guid holidayId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Holiday>> GetListAsync(Guid teamId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(Holiday model)
        {
            string sql = "INSERT INTO HOLIDAYS(HOLIDAY_ID, COMPANY_ID, NAME, START_DATE, END_DATE, IS_FULL_DAY, CREATED_AT, CREATED_BY)" +
                $" VALUES('{model.HolidayId}', '{model.CompanyId}', '{model.HolidayName}', '{model.StartDate}', '{model.EndDate}', '{model.IsFullDay}', '{model.CreatedAt}', '{model.CreatedBy}')";

            var affectedRow = await Connection.ExecuteAsync(sql);

            return affectedRow;
        }

        public async Task<int> InsertHolidayToTeams(Guid holidayId, IList<Guid> teamIds)
        {
            var sql = new StringBuilder("INSERT INTO HOLIDAY_TEAM(HOLIDAY_ID, TEAM_ID) VALUES");

            int teamCount = teamIds.Count;
            int currentCount = 0;
            foreach (var teamId in teamIds)
            {
                if (++currentCount == teamCount)
                    sql.Append($"('{holidayId}', '{teamId}');");
                else
                    sql.Append($"('{holidayId}', '{teamId}'),");
            }

            var affectedRow = await Connection.ExecuteAsync(sql.ToString());

            return affectedRow;
        }

        public Task<int> RemoveAsync(Guid holidayId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(IList<Guid> holidayId)
        {
            throw new NotImplementedException();
        }
    }
}
