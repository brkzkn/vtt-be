using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Holiday
{
    public class GetHolidayListQuery : QueryBase<IList<HolidayDto>>
    {
        public GetHolidayListQuery(Guid companyId)
        {
            CompanyId = companyId;
        }

        [Required]
        public Guid CompanyId { get; set; }
    }
}
