using System;
using System.Collections.Generic;

namespace VacationTracking.Api.Models
{
    public class TeamModel
    {
        public string Name { get; set; }
        public List<Guid> Approvers { get; set; }
        public List<Guid> Members { get; set; }
    }
}
