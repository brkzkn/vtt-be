using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class LeaveTypeRepository : BaseRepository, ILeaveTypeRepository
    {
        public LeaveTypeRepository(IDbConnection connection)
            : base(connection)
        {
        }

        public async Task<LeaveType> GetAsync(Guid companyId, Guid leaveTypeId)
        {
            string sql = $"SELECT * FROM LEAVE_TYPES WHERE LEAVE_TYPE_ID = '{leaveTypeId}' AND COMPANY_ID = '{companyId}'";
            LeaveType result = await Connection.QueryFirstOrDefaultAsync<LeaveType>(sql);

            return result;
        }

        public async Task<IEnumerable<LeaveType>> GetListAsync(Guid companyId)
        {
            string sql = $"SELECT * FROM LEAVE_TYPES WHERE COMPANY_ID = '{companyId}'";
            var result = await Connection.QueryAsync<LeaveType>(sql);

            return result;
        }

        public Task<int> InsertAsync(LeaveType model)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertHolidayToTeams(Guid leaveTypeId, IList<Guid> teamIds)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(Guid leaveTypeId, Guid companyId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveTeamHolidays(Guid leaveTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Guid leaveTypeId, LeaveType model)
        {
            throw new NotImplementedException();
        }
    }
}
