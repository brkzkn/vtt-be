using MediatR;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.LeaveType
{
    public class CreateLeaveTypeCommand : IRequest<LeaveTypeDto>, ILeaveTypeCommand
    {
        public CreateLeaveTypeCommand(int companyId,
                                      int userId,
                                      bool isHalfDaysActivated,
                                      bool isHideLeaveTypeName,
                                      string typeName,
                                      bool isApprovalRequired,
                                      int defaultDaysPerYear,
                                      bool isUnlimited,
                                      bool isReasonRequired,
                                      bool allowNegativeBalance,
                                      string color)
        {
            CompanyId = companyId;
            UserId = userId;
            IsHalfDaysActivated = isHalfDaysActivated;
            IsHideLeaveTypeName = isHideLeaveTypeName;
            LeaveTypeName = typeName;
            IsApproverRequired = isApprovalRequired;
            DefaultDaysPerYear = defaultDaysPerYear;
            IsUnlimited = isUnlimited;
            IsReasonRequired = isReasonRequired;
            IsAllowNegativeBalance = allowNegativeBalance;
            ColorCode = color;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public bool IsHalfDaysActivated { get; }
        public bool IsHideLeaveTypeName { get; }
        public string LeaveTypeName { get; }
        public bool IsApproverRequired { get; }
        public int DefaultDaysPerYear { get; }
        public bool IsUnlimited { get; }
        public bool IsReasonRequired { get; }
        public bool IsAllowNegativeBalance { get; }
        public string ColorCode { get; }
    }
}
