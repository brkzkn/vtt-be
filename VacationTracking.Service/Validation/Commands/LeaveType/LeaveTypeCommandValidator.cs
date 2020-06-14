using FluentValidation;
using VacationTracking.Domain.Commands.LeaveType;

namespace VacationTracking.Service.Validation.Commands.LeaveType
{
    public class LeaveTypeCommandValidator : AbstractValidator<ILeaveTypeCommand>
    {
        public LeaveTypeCommandValidator()
        {
            RuleFor(x => x.DefaultDaysPerYear).GreaterThan(0).When(x => x.IsUnlimited == false);
        }
    }
}
