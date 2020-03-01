namespace VacationTracking.Api.Models
{
	public class LeaveTypeModel
	{
		public bool IsHalfDaysActivated { get; set; }
		public bool IsHideLeaveTypeName { get; set; }
		public string TypeName { get; set; }
		public bool IsApprovalRequired { get; set; }
		public int DefaultDaysPerYear { get; set; }
		public bool IsUnlimited { get; set; }
		public bool IsReasonRequired { get; set; }
		public bool IsAllowNegativeBalance { get; set; }
		public string Color { get; set; }
		public bool IsActive { get; set; }
    }
}
