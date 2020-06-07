﻿using System;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface ILeaveTypeRepository
    {
        Task<int> UpdateAsync(Guid leaveTypeId, LeaveType model);
        Task<int> RemoveAsync(Guid leaveTypeId, Guid companyId);
    }
}
