using System;

namespace VacationTracking.Api.Models
{
    public class VacationModel
    {
        public string Reason { get; set; }
        public Guid LeaveTypeId { get; set; }
        public Guid UserId { get; set; }
        public bool IsHalfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Time { get; set; }
    }
}
