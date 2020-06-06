using FluentValidation;
using VacationTracking.Domain.Commands.Holiday;

namespace VacationTracking.Service.Validation.Commands.Holiday
{
    public class UpdateHolidayCommandValidator : AbstractValidator<UpdateHolidayCommand>
    {
        public UpdateHolidayCommandValidator()
        {
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(x => x.EndDate);

            RuleFor(x => x.Teams).NotEmpty().When(x => x.IsForAllTeams == false);
        }
    }
}
