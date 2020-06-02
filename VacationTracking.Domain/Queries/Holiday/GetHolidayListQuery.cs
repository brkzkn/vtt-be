using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Holiday
{
    public class GetHolidayListQuery : QueryBase<IList<HolidayDto>>
    {
        public GetHolidayListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        [Required]
        public int CompanyId { get; set; }
    }
}
