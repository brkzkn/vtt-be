namespace VacationTracking.Domain.Commands.LeaveType
{
    public interface ILeaveTypeCommand
    {
        int CompanyId { get; }
        int UserId { get; }
        bool IsHalfDaysActivated { get; }
        bool IsHideLeaveTypeName { get; }
        string LeaveTypeName { get; }
        bool IsApproverRequired { get; }
        int DefaultDaysPerYear { get; }
        bool IsUnlimited { get; }
        bool IsReasonRequired { get; }
        bool IsAllowNegativeBalance { get; }
        string ColorCode { get; }
    }
}
