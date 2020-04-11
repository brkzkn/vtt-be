using Dapper;
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
    }
}
