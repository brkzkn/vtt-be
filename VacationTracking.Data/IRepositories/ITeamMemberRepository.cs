using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember> GetAsync(Guid teamId, Guid userId);
        Task<IEnumerable<TeamMember>> GetListAsync(Guid teamId);
        Task<int> InsertAsync(TeamMember model);
        Task<int> InsertAsync(IEnumerable<TeamMember> model);
        Task<int> UpdateAsync(TeamMember model);
        Task<int> RemoveAsync(Guid teamId);
        Task<int> RemoveAsync(Guid teamId, Guid userId);
        Task<int> RemoveNotActiveMemberAsync();
        Task<int> SetAsNonMemberToOtherTeams(IEnumerable<Guid> userIds);
    }
}
