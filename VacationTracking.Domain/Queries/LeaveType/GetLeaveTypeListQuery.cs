using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.LeaveType
{
    public class GetLeaveTypeListQuery : QueryBase<IList<LeaveTypeDto>>
    {
        public GetLeaveTypeListQuery(Guid companyId)
        {
            CompanyId = companyId;
        }

        [Required]
        public Guid CompanyId { get; set; }
    }
}
