using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    [Table("CompanySetting")]
    public class CompanySetting
    {
        [Column("CompanyID")]
        public int CompanyId { get; set; }
        [Column("SettingID")]
        public int SettingId { get; set; }

        [Required]
        [StringLength(200)]
        public string SettingValue { get; set; }

        public virtual Company Company { get; set; }
        public virtual Setting Setting { get; set; }
    }
}
