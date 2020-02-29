using System;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Holiday
{
    public class GetHolidayQuery : QueryBase<HolidayDto>
    {
        public GetHolidayQuery(Guid holidayId, Guid companyId)
        {
            HolidayId = holidayId;
            CompanyId = companyId;
        }

        [Required]
        public Guid HolidayId { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
    }
}

