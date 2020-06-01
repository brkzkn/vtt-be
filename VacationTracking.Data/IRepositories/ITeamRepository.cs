using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<int> InsertAsync(Team model);
        Task<int> InsertAsync(IEnumerable<Team> model);
        Task<int> UpdateAsync(Team model);
        Task<int> RemoveAsync(Guid companyId, Guid teamId);
    }
}
