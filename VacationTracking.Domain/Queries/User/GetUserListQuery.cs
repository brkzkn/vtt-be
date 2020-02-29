using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.User
{
    public class GetUserListQuery : QueryBase<IList<UserDto>>
    {
        public GetUserListQuery(Guid companyId)
        {
            CompanyId = companyId;
        }

        public Guid CompanyId { get; set; }
    }
}
