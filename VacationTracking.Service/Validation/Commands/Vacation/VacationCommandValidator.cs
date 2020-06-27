using FluentValidation;
using VacationTracking.Domain.Commands.Vacation;

namespace VacationTracking.Service.Validation.Commands.Vacation
{
    public class VacationCommandValidator : AbstractValidator<IVacationCommand>
    {
        public VacationCommandValidator()
        {
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate);
            RuleFor(x => x.StartDate.Date).Equal(x => x.EndDate.Date).When(x => x.IsHalfDay);
        }
    }
}
