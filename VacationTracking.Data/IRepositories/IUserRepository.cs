using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetListAsync(Guid companyId);
        Task<int> InsertAsync(User model);
        Task<int> RemoveAsync(Guid userId, Guid companyId);
    }
}
