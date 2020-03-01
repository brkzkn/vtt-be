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

        public async Task<int> InsertAsync(LeaveType model)
        {
            string sql = "INSERT INTO LEAVE_TYPES(leave_type_id, company_id, is_default, is_half_days_activated, is_active, is_deleted, "
                         + "is_hide_leave_type_name, type_name, is_approver_required, default_days_per_year, is_unlimited, is_reason_required, "
                         + "is_allow_negative_balance, color_code, created_at, created_by)"
                         + $" VALUES('{model.LeaveTypeId}', '{model.CompanyId}', '{model.IsDefault}', '{model.IsHalfDaysActivated}', '{model.IsActive}', "
                         + $"'{model.IsDeleted}', '{model.IsHideLeaveTypeName}', '{model.TypeName}', '{model.IsApproverRequired}', '{model.DefaultDaysPerYear}', "
                         + $"'{model.IsUnlimited}', '{model.IsReasonRequired}', '{model.IsAllowNegativeBalance}', '{model.ColorCode}', "
                         + $"'{model.CreatedAt}', '{model.CreatedBy}')";

            var affectedRow = await Connection.ExecuteAsync(sql);

            return affectedRow;
        }

        public async Task<bool> IsLeaveTypeExistAsync(Guid companyId, string name)
        {
            var exists = await Connection.ExecuteScalarAsync<bool>($"select count(1) from Leave_Types where company_id='{companyId}' and type_name = '{name}'");

            return exists;
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
