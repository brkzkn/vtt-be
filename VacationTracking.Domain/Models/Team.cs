using System;

namespace VacationTracking.Domain.Models
{
    public class Teams : BaseModel
    {
        public Guid TeamId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
    }

}
