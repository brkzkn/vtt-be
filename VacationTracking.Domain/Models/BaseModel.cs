using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class BaseModel
    {
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_by")]
        public Guid CreatedBy { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("updated_by", TypeName = "timestamp with time zone")]
        public Guid? UpdatedBy { get; set; }
    }
}
