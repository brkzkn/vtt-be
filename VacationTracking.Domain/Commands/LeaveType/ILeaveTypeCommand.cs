namespace VacationTracking.Domain.Commands.LeaveType
{
	public interface ILeaveTypeCommand
    {
		int CompanyId { get; set; }
		int UserId { get; set; }
		bool IsHalfDaysActivated { get; set; }
		bool IsHideLeaveTypeName { get; set; }
		string LeaveTypeName { get; set; }
		bool IsApproverRequired { get; set; }
		int DefaultDaysPerYear { get; set; }
		bool IsUnlimited { get; set; }
		bool IsReasonRequired { get; set; }
		bool IsAllowNegativeBalance { get; set; }
		string ColorCode { get; set; }
	}
}
