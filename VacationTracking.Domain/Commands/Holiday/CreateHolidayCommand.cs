using MediatR;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class CreateHolidayCommand : IRequest<HolidayDto>
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

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Teams { get; set; }
        public bool IsForAllTeams { get; set; }
        public bool IsFullDay{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
