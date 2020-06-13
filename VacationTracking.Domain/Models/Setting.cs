using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacationTracking.Domain.Enums;

namespace VacationTracking.Domain.Models
{
    [Table("Setting")]
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

        [Column(TypeName = "nvarchar(20)")]
        public SettingType SettingType { get; set; }

        public virtual ICollection<CompanySetting> CompanySettings { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
    }
}
