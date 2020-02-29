using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.User
{
    public class GetUserQuery : QueryBase<UserDto>
    {
        public GetUserQuery(Guid userId, Guid companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }

        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
