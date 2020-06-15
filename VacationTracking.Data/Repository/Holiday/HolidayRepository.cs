using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using HolidayDb = VacationTracking.Domain.Models.Holiday;

namespace VacationTracking.Data.Repository.Holiday
{
    public static class HolidayRepository
    {
        public static async Task<bool> IsDateOverlapHolidaysAsync(this IRepository<HolidayDb> repository,
                                                                  int companyId,
                                                                  DateTime startDate,
                                                                  DateTime endDate)
        {
            var result = await repository.Queryable()
                                         .AnyAsync(x => x.CompanyId == companyId 
                                                     && startDate.Date <= x.EndDate.Date && endDate.Date >= x.StartDate.Date);

            return result;
        }
    }
}
