using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<Team> GetAsync(Guid teamId, Guid companyId);
        Task<IEnumerable<Team>> GetListAsync(Guid companyId);
        Task<int> InsertAsync(Team model);
        Task<int> InsertAsync(IEnumerable<Team> model);
        Task<int> UpdateAsync(Team model);
        Task<int> RemoveAsync(Guid companyId, Guid teamId);
    }
}
