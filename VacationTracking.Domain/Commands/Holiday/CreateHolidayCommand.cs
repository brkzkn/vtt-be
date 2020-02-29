using MediatR;
using System;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class CreateHolidayCommand : IRequest<HolidayDto>
    {
        public CreateHolidayCommand(Guid companyId, Guid userId, IList<Guid> teams, DateTime endDate, DateTime startDate, string name, bool isForAllTeams, bool isFullDay)
        {
            Teams = teams;
            EndDate = endDate;
            StartDate = startDate;
            Name = name;
            IsForAllTeams = isForAllTeams;
            IsFullDay = isFullDay;
            CompanyId = companyId;
            UserId = userId;
        }

        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public IList<Guid> Teams { get; set; }
        public bool IsForAllTeams { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFullDay { get; set; }
    }
}
