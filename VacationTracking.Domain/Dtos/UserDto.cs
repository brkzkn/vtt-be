using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class UserDto : BaseDto
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("companyId")]
        public Guid CompanyId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("employeeSince")]
        public DateTime EmployeeSince { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }
}
