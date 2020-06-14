using MediatR;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class CreateHolidayCommand : IRequest<HolidayDto>, IHolidayCommand
    {
        public CreateHolidayCommand(int companyId,
                                    int userId,
                                    IEnumerable<int> teams,
                                    DateTime endDate,
                                    DateTime startDate,
                                    string name,
                                    bool isForAllTeams,
                                    bool isFullDay)
        {
            Teams = teams;
            EndDate = endDate;
            StartDate = startDate;
            Name = name;
            IsForAllTeams = isForAllTeams;
            CompanyId = companyId;
            UserId = userId;
            IsFullDay = isFullDay;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public string Name { get; }
        public IEnumerable<int> Teams { get; }
        public bool IsForAllTeams { get; }
        public bool IsFullDay { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
    }
}
