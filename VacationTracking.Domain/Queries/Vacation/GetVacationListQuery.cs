using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Vacation
{
    public class GetVacationListQuery : QueryBase<IList<VacationDto>>
    {
        [JsonConstructor]
        public GetVacationListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        [JsonProperty("id")]
        [Required]
        public int CompanyId { get; set; }
    }
}
