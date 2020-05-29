using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class Setting
    {
        public Setting()
        {
            CompanySettings = new HashSet<CompanySetting>();
            UserSettings = new HashSet<UserSetting>();
        }

        [Column("SettingID")]
        public int SettingId { get; set; }

        [Required]
        [StringLength(150)]
        public string SettingKey { get; set; }

        [Required]
        [StringLength(150)]
        public string DefaultValue { get; set; }
        public bool? IsUserSetting { get; set; }

        public virtual ICollection<CompanySetting> CompanySettings { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
    }
}
