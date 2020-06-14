using MediatR;

namespace VacationTracking.Domain.Commands.Holiday
{
    public class DeleteHolidayCommand : IRequest<bool>
    {
        public DeleteHolidayCommand(int holidayId, int companyId)
        {
            HolidayId = holidayId;
            CompanyId = companyId;
        }

        public int HolidayId { get; set; }
        public int CompanyId { get; set; }
    }
}
