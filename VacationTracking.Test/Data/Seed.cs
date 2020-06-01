using System;
using System.Collections.Generic;
using VacationTracking.Domain.Enums;
using VacationTracking.Domain.Models;

namespace VacationTracking.Test.Data
{
    public static class Seed
    {
        public static ICollection<User> Users()
            => new List<User>()
            {
                new User { UserId = 1,  CompanyId = 1, Email = "burak.ozkan@vt.com", CreatedAt = DateTime.Now, EmployeeSince = DateTime.UtcNow.AddYears(-3).Date, FullName = "Burak Ozkan", Status = UserStatus.Active, AccountType = AccountType.Admin},
                new User { UserId = 2,  CompanyId = 1, Email = "kubilay.akca@vt.com", CreatedAt = DateTime.Now, EmployeeSince = DateTime.UtcNow.AddYears(-3).AddMonths(-2).Date, FullName = "Kubilay Akca", Status = UserStatus.Active, AccountType = AccountType.Admin},
                new User { UserId = 3,  CompanyId = 2, Email = "zehra.karakas@vt.com", CreatedAt = DateTime.Now, EmployeeSince = DateTime.UtcNow.AddYears(-3).AddMonths(-2).Date, FullName = "Zehra Karakas", Status = UserStatus.Active, AccountType = AccountType.Admin},
            };

        public static ICollection<Team> Teams()
            => new List<Team>()
            {
                new Team { TeamId = 1, CompanyId = 1, TeamName = "Fenerbahce", CreatedAt = DateTime.Now, CreatedBy = 1},
                new Team { TeamId = 2, CompanyId = 1, TeamName = "Roma", CreatedAt = DateTime.Now, CreatedBy = 1},
                new Team { TeamId = 3, CompanyId = 2, TeamName = "Chelsea", CreatedAt = DateTime.Now, CreatedBy = 1},
            };

        public static ICollection<Company> Companies()
            => new List<Company>()
            {
                new Company { CompanyId = 1, CompanyName="Facebook", Country="Turkey", Address="Istanbul", CreatedAt= DateTime.Now, CreatedBy=-1},
                new Company { CompanyId = 2, CompanyName="Google", Country="Italy", Address="Roma", CreatedAt= DateTime.Now, CreatedBy=-1},
                new Company { CompanyId = 3, CompanyName="Twitter", Country="USA", Address="Houston", CreatedAt= DateTime.Now, CreatedBy=-1}
            };

        public static ICollection<TeamMember> TeamMembers()
            => new List<TeamMember>()
            {
                new TeamMember() {TeamId = 1, UserId  = 1, IsApprover=true, IsMember=true },
                new TeamMember() {TeamId=1, UserId=2, IsApprover=false, IsMember=true},
                new TeamMember() {TeamId=2, UserId=3, IsApprover=true, IsMember=true},
            };
    }
}
