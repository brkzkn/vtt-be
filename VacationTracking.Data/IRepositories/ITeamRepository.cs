using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<Domain.Models.Team> GetAsync(Guid teamId, Guid companyId);
        Task<IList<Domain.Models.Team>> GetListAsync(Guid companyId);
        Task<Domain.Models.Team> CreateTeamAsync(Team model);
        Task<bool> DeleteTeamAsync(Guid companyId, Guid teamId);
    }
}
