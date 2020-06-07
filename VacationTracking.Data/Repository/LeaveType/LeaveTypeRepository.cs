using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Data.Repository.LeaveType
{
    public static class LeaveTypeRepository
    {
        public static async Task<bool> IsLeaveTypeNameExistAsync(this IRepository<LeaveTypeDb> repository,
                                                                  int companyId,
                                                                  string leaveTypeName)
        {
            var result = await repository.Queryable()
                                         .AnyAsync(x => x.CompanyId == companyId 
                                                     && x.IsDeleted == false
                                                     && x.LeaveTypeName == leaveTypeName);

            return result;
        }
    }
}
