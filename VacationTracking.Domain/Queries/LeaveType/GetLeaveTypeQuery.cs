using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.LeaveType
{
    public class GetLeaveTypeQuery : QueryBase<LeaveTypeDto>
    {
        public GetLeaveTypeQuery(Guid leaveTypeId, Guid companyId)
        {
            LeaveTypeId = leaveTypeId;
            CompanyId = companyId;
        }

        public Guid LeaveTypeId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
