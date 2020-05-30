using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbConnection connection): base(connection)
        {
        }

        public async Task<User> GetAsync(Guid companyId, Guid userId)
        {
            throw new NotImplementedException();
            //string sql = $"SELECT * FROM USERS WHERE USER_ID = '{userId}' AND COMPANY_ID = '{companyId}'";
            //var result = await Connection.QueryFirstOrDefaultAsync<User>(sql);

            //return result;
        }

        public async Task<IEnumerable<User>> GetListAsync(Guid companyId)
        {
            throw new NotImplementedException();

            //string sql = $"SELECT * FROM USERS WHERE COMPANY_ID = '{companyId}'";
            //var result = await Connection.QueryAsync<User>(sql);

            //return result;
        }

        public Task<int> InsertAsync(User model)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(Guid userId, Guid companyId)
        {
            throw new NotImplementedException();
        }
    }
}
