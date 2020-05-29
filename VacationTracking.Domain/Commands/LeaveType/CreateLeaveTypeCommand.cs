using MediatR;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.LeaveType
{
	public class CreateLeaveTypeCommand: IRequest<LeaveTypeDto>
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
			TypeName = typeName;
			IsApproverRequired = isApprovalRequired;
			DefaultDaysPerYear = defaultDaysPerYear;
			IsUnlimited = isUnlimited;
			IsReasonRequired = isReasonRequired;
			IsAllowNegativeBalance = allowNegativeBalance;
			Color = color;
		}

		public int CompanyId { get; set; }
		public int UserId { get; set; }
		public bool IsHalfDaysActivated { get; set; }
		public bool IsHideLeaveTypeName { get; set; }
		public string TypeName { get; set; }
		public bool IsApproverRequired { get; set; }
		public int DefaultDaysPerYear { get; set; }
		public bool IsUnlimited { get; set; }
		public bool IsReasonRequired { get; set; }
		public bool IsAllowNegativeBalance { get; set; }
		public string Color { get; set; }
	}
}
