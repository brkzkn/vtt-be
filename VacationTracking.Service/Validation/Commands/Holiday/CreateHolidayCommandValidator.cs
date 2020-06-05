using FluentValidation;
using VacationTracking.Domain.Commands.Holiday;

namespace VacationTracking.Service.Validation.Commands.Holiday
{
    public class CreateHolidayCommandValidator : AbstractValidator<CreateHolidayCommand>
    {
        public CreateHolidayCommandValidator()
        {
            RuleFor(x => x.StartDate).GreaterThan(x => x.EndDate);
        }
    }
}
