using System;
using System.Collections.Generic;

namespace VacationTracking.Api.Models
{
    public class HolidayModel
    {
        public string Name { get; set; }
        public List<int> Teams { get; set; }
        public bool IsForAllTeams { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFullDay { get; set; }
    }
}
