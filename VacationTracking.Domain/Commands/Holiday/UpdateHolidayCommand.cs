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

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int HolidayId { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Teams { get; set; }
        public bool IsForAllTeams { get; set; }
        public bool IsFullDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
