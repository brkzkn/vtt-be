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

        public async Task<int> RemoveAsync(Guid leaveTypeId, Guid companyId)
        {
            throw new NotImplementedException();

            //string query = "UPDATE leave_types SET " +
            //    $"is_deleted = 'true' " +
            //    $"WHERE leave_type_id = '{leaveTypeId}' AND COMPANY_ID = '{companyId}';";

            //var affectedRow = await Connection.ExecuteAsync(query);

            //return affectedRow;
        }

        public async Task<int> UpdateAsync(Guid leaveTypeId, LeaveType model)
        {
            throw new NotImplementedException();

            //if (model == null)
            //    throw new ArgumentNullException(nameof(LeaveType));
           
            //string query = "UPDATE leave_types SET " +
            //    $"is_half_days_activated = '{model.IsHalfDaysActivated}', " +
            //    $"is_active = '{model.IsActive}', " +
            //    $"is_hide_leave_type_name = '{model.IsHideLeaveTypeName}', " +
            //    $"type_name = '{model.TypeName}', " +
            //    $"is_approver_required = '{model.IsApproverRequired}', " +
            //    $"default_days_per_year = '{model.DefaultDaysPerYear}', " +
            //    $"is_unlimited = '{model.IsUnlimited}', " +
            //    $"is_reason_required = '{model.IsReasonRequired}', " +
            //    $"is_allow_negative_balance = '{model.IsAllowNegativeBalance}', " +
            //    $"color_code = '{model.ColorCode}', " +
            //    $"updated_at = '{model.ModifiedAt}', " +
            //    $"updated_by = '{model.ModifiedBy}' " +
            //    $"WHERE leave_type_id = '{model.LeaveTypeId}' AND COMPANY_ID = '{model.CompanyId}';";

            //var affectedRow = await Connection.ExecuteAsync(query);

            //return affectedRow;
        }
    }
}
