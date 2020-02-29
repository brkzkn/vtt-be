using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface IHolidayRepository
    {
        Task<Holiday> GetAsync(Guid companyId, Guid holidayId);
        Task<IEnumerable<Holiday>> GetListAsync(Guid companyId);
        Task<int> UpdateAsync(Guid holidayId, Holiday model);
        Task<int> InsertAsync(Holiday model);
        Task<int> InsertHolidayToTeams(Guid holidayId, IList<Guid> teamIds);
        Task<int> RemoveAsync(Guid holidayId, Guid companyId);
        Task<int> RemoveTeamHolidays(Guid holidayId);
    }
}
