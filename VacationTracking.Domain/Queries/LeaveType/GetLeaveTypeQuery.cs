using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.LeaveType
{
    public class GetLeaveTypeQuery : QueryBase<LeaveTypeDto>
    {
        public GetLeaveTypeQuery(int leaveTypeId, int companyId)
        {
            LeaveTypeId = leaveTypeId;
            CompanyId = companyId;
        }

        public int LeaveTypeId { get; set; }
        public int CompanyId { get; set; }
    }
}
