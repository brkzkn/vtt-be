﻿using System.Threading;
using System.Threading.Tasks;

namespace VacationTracking.Data.UnitOfWork
{
    public interface IUnitOfWork 
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
