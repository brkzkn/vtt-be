using MediatR;

namespace VacationTracking.Domain.Commands.LeaveType
{
    public class DeleteLeaveTypeCommand : IRequest<bool>
    {
        public DeleteLeaveTypeCommand(int leaveTypeId, int companyId)
        {
            LeaveTypeId = leaveTypeId;
            CompanyId = companyId;
        }

        public int LeaveTypeId { get; }
        public int CompanyId { get; }
    }
}
