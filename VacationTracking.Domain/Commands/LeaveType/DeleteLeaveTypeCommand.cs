using MediatR;
using System;

namespace VacationTracking.Domain.Commands.LeaveType
{
    public class DeleteLeaveTypeCommand : IRequest<bool>
    {
        public DeleteLeaveTypeCommand(Guid leaveTypeId, Guid companyId)
        {
            LeaveTypeId = leaveTypeId;
            CompanyId = companyId;
        }

        public Guid LeaveTypeId { get; set; }
        public Guid CompanyId { get; set; }

    }
}
