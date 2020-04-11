using Newtonsoft.Json;

namespace VacationTracking.Api.Models
{
    public class CompanySettingsModel
    {
        [JsonProperty("date_format")]
        public string DateFormat { get; set; }

        [JsonProperty("transfer_days")]
        public bool TransferDays { get; set; }

        [JsonProperty("first_working_month")]
        public int FirstWorkingMonth { get; set; }

        [JsonProperty("first_working_day")]
        public int FirstWorkingDay { get; set; }
    }
}
