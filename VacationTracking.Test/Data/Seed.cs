using System;
using System.Collections.Generic;
using VacationTracking.Domain.Models;

namespace VacationTracking.Test.Data
{
    public static class Seed
    {
        public static ICollection<User> Users()
            => new List<User>()
            {
                new User { UserId = Guid.NewGuid(),  CompanyId = Guid.NewGuid(), Email = "burak.ozkan@vt.com", CreatedAt = DateTime.Now, EmployeeSince = DateTime.UtcNow.AddYears(-3).Date, FullName = "Burak Ozkan", Status = "active", AccountType = "admin"}
            };
    }
}
