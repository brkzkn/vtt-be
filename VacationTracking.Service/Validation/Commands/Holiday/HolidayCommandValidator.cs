using FluentValidation;
using VacationTracking.Domain.Commands.Holiday;

namespace VacationTracking.Service.Validation.Commands.Holiday
{
    public class HolidayCommandValidator : AbstractValidator<IHolidayCommand>
    {
        public HolidayCommandValidator()
        {
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(x => x.EndDate);

            RuleFor(x => x.Teams).NotEmpty().When(x => x.IsForAllTeams == false);
        }
    }
}
