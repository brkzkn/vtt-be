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

        public async Task<Holiday> GetAsync(Guid companyId, Guid holidayId)
        {
            string sql = $"SELECT * FROM HOLIDAYS WHERE HOLIDAY_ID = '{holidayId}' AND COMPANY_ID = '{companyId}'";
            Holiday result = await Connection.QueryFirstOrDefaultAsync<Holiday>(sql);

            return result;
        }

        public async Task<IEnumerable<Holiday>> GetListAsync(Guid companyId)
        {
            string sql = "SELECT * FROM HOLIDAYS as h  " +
                "JOIN HOLIDAY_TEAM as ht on h.holiday_id = ht.holiday_id " +
                "JOIN TEAMS as t on t.team_id = ht.team_id " +
                $"WHERE h.COMPANY_ID = '{companyId}'";

            Dictionary<Guid, Holiday> holidaysDictionary = new Dictionary<Guid, Holiday>();

            var result = await Connection.QueryAsync<Holiday, HolidayTeam, Team, Holiday>(sql, map: (h, ht, t) =>
            {
                if (!holidaysDictionary.TryGetValue(h.HolidayId, out Holiday holidayEntry))
                {
                    holidayEntry = h;
                    holidayEntry.Teams = new List<Team>();
                    holidaysDictionary.Add(h.HolidayId, holidayEntry);
                }
                holidayEntry.Teams.Add(t);
                return holidayEntry;

            }, splitOn: "holiday_id, team_id");

            return holidaysDictionary.Values.AsList();
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

        public async Task<int> RemoveAsync(Guid holidayId, Guid companyId)
        {
            string sql = $"DELETE FROM HOLIDAYS WHERE HOLIDAY_ID = '{holidayId}' and COMPANY_ID = '{companyId}'";
            var affectedRow = await Connection.ExecuteAsync(sql);
            return affectedRow;
        }

        public async Task<int> RemoveTeamHolidays(Guid holidayId)
        {
            string sql = $"DELETE FROM HOLIDAY_TEAM WHERE HOLIDAY_ID = '{holidayId}'";
            var affectedRow = await Connection.ExecuteAsync(sql);
            return affectedRow;
        }
    }
}
