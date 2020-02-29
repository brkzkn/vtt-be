using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface IHolidayRepository
    {
        Task<Holiday> GetAsync(Guid companyId, Guid holidayId);
        Task<IEnumerable<Holiday>> GetListAsync(Guid teamId);
        Task<int> InsertAsync(Holiday model);
        Task<int> InsertHolidayToTeams(Guid holidayId, IList<Guid> teamIds);
        Task<int> RemoveAsync(Guid holidayId);
        Task<int> RemoveAsync(IList<Guid> holidayId);
    }
}
