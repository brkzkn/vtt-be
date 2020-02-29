using MediatR;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class UpdateHolidayCommand : IRequest<HolidayDto>
    {
        public UpdateHolidayCommand(Guid companyId,
                                    Guid holidayId,
                                    Guid userId,
                                    string name,
                                    DateTime startDate,
                                    DateTime endDate,
                                    bool isFullday,
                                    bool isForAllTeams,
                                    List<Guid> teams)
        {
            CompanyId = companyId;
            HolidayId = holidayId;
            UserId = userId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            IsFullDay = isFullday;
            IsForAllTeams = isForAllTeams;
            Teams = teams;
        }
        public bool IsForAllTeams { get; set; }
        public Guid HolidayId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFullDay { get; set; }
        public List<Guid> Teams { get; set; }
    }
}
