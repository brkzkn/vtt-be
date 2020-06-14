using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.User
{
    public class GetUserListQuery : QueryBase<IEnumerable<UserDto>>
    {
        public GetUserListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        public int CompanyId { get; set; }
    }
}
