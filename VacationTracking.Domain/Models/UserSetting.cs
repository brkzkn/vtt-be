using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class UserSetting
    {
        [Column("UserID")]
        public int UserId { get; set; }

        [Column("SettingID")]
        public int SettingId { get; set; }

        [Required]
        [StringLength(150)]
        public string SettingValue { get; set; }

        public virtual Setting Setting { get; set; }
        public virtual User User { get; set; }
    }
}
