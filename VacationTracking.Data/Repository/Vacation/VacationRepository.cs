using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VacationDb = VacationTracking.Domain.Models.Vacation;

namespace VacationTracking.Data.Repository.Vacation
{
    public static class VacationRepository
    {
        public static async Task<bool> IsDateOverlapExistingVacationAsync(this IRepository<VacationDb> repository,
                                                                          int userId,
                                                                          DateTime startDate,
                                                                          DateTime endDate,
                                                                          bool isHalfDay)
        {
            bool result = false;
            if (isHalfDay)
            {
                result = await repository.Queryable().AnyAsync(x => x.UserId == userId
                                                                 && startDate.Date <= x.EndDate.Date
                                                                 && endDate.Date >= x.StartDate.Date
                                                                 && x.IsHalfDay == false);
            }
            else
            {
                result = await repository.Queryable().AnyAsync(x => x.UserId == userId
                                                                 && startDate.Date <= x.EndDate.Date
                                                                 && endDate.Date >= x.StartDate.Date);
            }
            

            return result;
        }
    }
}
