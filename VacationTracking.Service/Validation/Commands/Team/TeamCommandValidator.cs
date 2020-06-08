using FluentValidation;
using VacationTracking.Domain.Commands.Team;

namespace VacationTracking.Service.Validation.Commands.Team
{
    public class TeamCommandValidator : AbstractValidator<ITeamCommand>
    {
        public TeamCommandValidator()
        {
            RuleFor(x => x.Approvers.Count).GreaterThan(0);
            RuleFor(x => x.Members.Count).GreaterThan(0);
        }
    }
}
