using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Holiday
{
    public class GetHolidayQuery : QueryBase<HolidayDto>
    {
        public GetHolidayQuery(int holidayId, int companyId)
        {
            HolidayId = holidayId;
            CompanyId = companyId;
        }

        [Required]
        public int HolidayId { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}

