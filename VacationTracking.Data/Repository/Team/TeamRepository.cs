using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Data.Repository.Team
{
    public static class TeamRepository
    {
        public static async Task<bool> IsTeamNameExistAsync(this IRepository<TeamDb> repository,
                                                            int companyId,
                                                            string teamName)
        {
            var result = await repository.Queryable()
                                         .AnyAsync(x => x.CompanyId == companyId
                                                     && x.TeamName == teamName);

            return result;
        }
    }
}
