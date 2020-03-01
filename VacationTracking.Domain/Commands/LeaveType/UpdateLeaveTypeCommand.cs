using MediatR;
using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.LeaveType
{
	public class UpdateLeaveTypeCommand : IRequest<LeaveTypeDto>
    {
		public UpdateLeaveTypeCommand(Guid companyId,
								Guid userId,
								Guid leaveTypeId,
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
			TypeName = typeName;
			IsApproverRequired = isApprovalRequired;
			DefaultDaysPerYear = defaultDaysPerYear;
			IsUnlimited = isUnlimited;
			IsReasonRequired = isReasonRequired;
			IsAllowNegativeBalance = allowNegativeBalance;
			Color = color;
			IsActive = isActive;
		}

		public Guid CompanyId { get; set; }
		public Guid UserId { get; set; }
		public Guid LeaveTypeId { get; set; }
		public bool IsHalfDaysActivated { get; set; }
		public bool IsHideLeaveTypeName { get; set; }
		public string TypeName { get; set; }
		public bool IsApproverRequired { get; set; }
		public int DefaultDaysPerYear { get; set; }
		public bool IsUnlimited { get; set; }
		public bool IsReasonRequired { get; set; }
		public bool IsAllowNegativeBalance { get; set; }
		public string Color { get; set; }
		public bool IsActive { get; set; }
	}
}
