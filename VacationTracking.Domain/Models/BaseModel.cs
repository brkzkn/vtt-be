using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTracking.Domain.Models
{
    public class BaseModel
    {
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
