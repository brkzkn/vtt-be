using MediatR;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.LeaveType
{
    public class UpdateLeaveTypeCommand : IRequest<LeaveTypeDto>, ILeaveTypeCommand
    {
        public UpdateLeaveTypeCommand(int companyId,
                                      int userId,
                                      int leaveTypeId,
                                      bool isHalfDaysActivated,
                                      bool isHideLeaveTypeName,
                                      string typeName,
                                      bool isApprovalRequired,
                                      int defaultDaysPerYear,
                                      bool isUnlimited,
                                      bool isReasonRequired,
                                      bool allowNegativeBalance,
                                      string color,
                                      bool isActive)
        {
            CompanyId = companyId;
            UserId = userId;
            LeaveTypeId = leaveTypeId;
            IsHalfDaysActivated = isHalfDaysActivated;
            IsHideLeaveTypeName = isHideLeaveTypeName;
            LeaveTypeName = typeName;
            IsApproverRequired = isApprovalRequired;
            DefaultDaysPerYear = defaultDaysPerYear;
            IsUnlimited = isUnlimited;
            IsReasonRequired = isReasonRequired;
            IsAllowNegativeBalance = allowNegativeBalance;
            ColorCode = color;
            IsActive = isActive;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public int LeaveTypeId { get; }
        public bool IsHalfDaysActivated { get; }
        public bool IsHideLeaveTypeName { get; }
        public string LeaveTypeName { get; }
        public bool IsApproverRequired { get; }
        public int DefaultDaysPerYear { get; }
        public bool IsUnlimited { get; }
        public bool IsReasonRequired { get; }
        public bool IsAllowNegativeBalance { get; }
        public string ColorCode { get; }
        public bool IsActive { get; }
    }
}
