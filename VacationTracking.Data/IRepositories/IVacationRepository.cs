﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data.IRepositories
{
    public interface IVacationRepository
    {
        Task<int> InsertAsync(Vacation model);
        Task<int> UpdateStatusAsync(Vacation model);
        Task<IEnumerable<Vacation>> GetListAsync(Guid companyId);
    }
}