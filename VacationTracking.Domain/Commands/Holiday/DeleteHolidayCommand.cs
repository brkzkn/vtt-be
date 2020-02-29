using MediatR;
using System;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class DeleteHolidayCommand : IRequest<bool>
    {
        public DeleteHolidayCommand(Guid holidayId, Guid companyId)
        {
            HolidayId = holidayId;
            CompanyId = companyId;
        }

        public Guid HolidayId { get; set; }
        public Guid CompanyId { get; set; }

    }
}
