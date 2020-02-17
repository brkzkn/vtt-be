using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries
{
    public class GetTeamQuery : QueryBase<TeamDto>
    {
        public GetTeamQuery()
        {
        }

        [JsonConstructor]
        public GetTeamQuery(Guid customerId)
        {
            CustomerId = customerId;
        }

        [JsonProperty("id")]
        [Required]
        public Guid CustomerId { get; set; }
    }
}
