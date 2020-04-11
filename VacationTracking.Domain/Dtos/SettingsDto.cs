using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Dtos
{
    public class SettingsDto
    {
        [JsonProperty("settingId")]
        public Guid SettingId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("isUserSetting")]
        public bool IsUserSetting { get; set; }

        [JsonProperty("settingValue")]
        public string settingValue { get; set; }

    }
}
