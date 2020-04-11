using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface IVacationRepository
    {
        Task<int> InsertAsync(Vacation model);
    }
}
