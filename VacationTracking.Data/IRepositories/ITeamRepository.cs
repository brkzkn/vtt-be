using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<Domain.Models.Team> GetAsync(Guid teamId);
        Task<IList<Domain.Models.Team>> GetListAsync(Guid companyId);
    }
}
