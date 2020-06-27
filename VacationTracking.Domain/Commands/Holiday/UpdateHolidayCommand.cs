using MediatR;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class UpdateHolidayCommand : IRequest<HolidayDto>, IHolidayCommand
    {
        public UpdateHolidayCommand(int companyId,
                                    int holidayId,
                                    int userId,
                                    string name,
                                    DateTime startDate,
                                    DateTime endDate,
                                    List<int> teams,
                                    bool isForAllTeams,
                                    bool isFullDay)
        {
            CompanyId = companyId;
            HolidayId = holidayId;
            UserId = userId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            IsForAllTeams = isForAllTeams;
            IsFullDay = isFullDay;
            Teams = teams;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public int HolidayId { get; }
        public string Name { get; }
        public IEnumerable<int> Teams { get; }
        public bool IsForAllTeams { get; }
        public bool IsFullDay { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
    }
}
