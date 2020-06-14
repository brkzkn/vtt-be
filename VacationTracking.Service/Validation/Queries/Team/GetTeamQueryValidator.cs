using FluentValidation;
using VacationTracking.Domain.Queries.Team;

namespace VacationTracking.Service.Validation.Queries.Team
{
    public class GetTeamQueryValidator : AbstractValidator<GetTeamQuery>
    {
        public GetTeamQueryValidator()
        {
            RuleFor(x => x.UserId).Must((query, userId) => HasPermission(userId, query.CompanyId));
        }

        /*
         * For custom validation reference;
         * https://docs.fluentvalidation.net/en/latest/custom-validators.html
         * 
         */
        public bool HasPermission(int userId, int companyId)
        {
            return true;
        }
    }
}
