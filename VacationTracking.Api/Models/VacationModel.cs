using System;

namespace VacationTracking.Api.Models
{
    public class VacationModel
    {
        /// <summary>
        /// User can type reason of vacation
        /// </summary>
        public string Reason { get; set; }
        public Guid LeaveTypeId { get; set; }
        public bool IsHalfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
