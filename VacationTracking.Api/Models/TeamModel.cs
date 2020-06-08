using System.Collections.Generic;

namespace VacationTracking.Api.Models
{
    public class TeamModel
    {
        public string Name { get; set; }
        public List<int> Approvers { get; set; }
        public List<int> Members { get; set; }
    }
}
