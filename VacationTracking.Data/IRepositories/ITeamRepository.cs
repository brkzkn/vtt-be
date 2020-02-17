using System;
using System.Threading.Tasks;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<Domain.Models.Team> GetAsync(Guid id);
    }
}
