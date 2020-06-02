using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.LeaveType
{
    public class GetLeaveTypeListQuery : QueryBase<IList<LeaveTypeDto>>
    {
        public GetLeaveTypeListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        [Required]
        public int CompanyId { get; set; }
    }
}
