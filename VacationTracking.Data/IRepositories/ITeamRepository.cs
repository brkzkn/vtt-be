using System;
using System.Threading.Tasks;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamRepository 
    {
        Task<Domain.Models.Teams> GetAsync(Guid id);
    }
}
