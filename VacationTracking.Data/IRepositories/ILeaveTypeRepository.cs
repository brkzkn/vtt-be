﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ILeaveTypeRepository
    {
        Task<LeaveType> GetAsync(Guid companyId, Guid leaveTypeId);
        Task<IEnumerable<LeaveType>> GetListAsync(Guid companyId);
        Task<int> UpdateAsync(Guid leaveTypeId, LeaveType model);
        Task<int> InsertAsync(LeaveType model);
        Task<int> InsertHolidayToTeams(Guid leaveTypeId, IList<Guid> teamIds);
        Task<int> RemoveAsync(Guid leaveTypeId, Guid companyId);
        Task<int> RemoveTeamHolidays(Guid leaveTypeId);
    }
}
