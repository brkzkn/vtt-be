using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Commands.Holiday
{
    public interface IHolidayCommand
    {
        int CompanyId { get; set; }
        int UserId { get; set; }
        string Name { get; set; }
        IEnumerable<int> Teams { get; set; }
        bool IsForAllTeams { get; set; }
        bool IsFullDay { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}
