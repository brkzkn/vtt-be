using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Commands.Holiday
{
    public interface IHolidayCommand
    {
        int CompanyId { get; }
        int UserId { get; }
        string Name { get; }
        IEnumerable<int> Teams { get; }
        bool IsForAllTeams { get; }
        bool IsFullDay { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
    }
}
