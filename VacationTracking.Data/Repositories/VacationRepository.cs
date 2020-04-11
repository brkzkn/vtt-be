using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class VacationRepository : BaseRepository, IVacationRepository
    {
        public VacationRepository(IDbConnection connection)
            : base(connection)
        {
        }

        public async Task<int> InsertAsync(Vacation model)
        {
            string insertSql = "INSERT INTO vacations(vacation_id, user_id, leave_type_id, vacation_status, start_date, end_date, reason, created_at, created_by)" +
                $" VALUES('{model.VacationId}', '{model.UserId}', '{model.LeaveTypeId}', '{model.Status}', '{model.StartDate}', '{model.EndDate}', " +
                $"'{model.Reason}', '{model.CreatedAt}', '{model.CreatedBy}')";

            var affectedRow = await Connection.ExecuteAsync(insertSql);

            return affectedRow;
        }

        public async Task<int> UpdateStatusAsync(Vacation model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(Team));

            string query = "UPDATE VACATIONS SET " +
                $"VACATION_STATUS = '{model.Status}', " +
                $"APPROVER_ID = '{model.ApproverId}', " +
                $"RESPONSE = '{model.Response}', " +
                $"UPDATED_AT = '{model.UpdatedAt}', " +
                $"UPDATED_BY = '{model.UpdatedBy}' " +
                $"WHERE VACATION_ID = '{model.VacationId}';";

            var affectedRow = await Connection.ExecuteAsync(query);

            return affectedRow;
        }
    }
}
