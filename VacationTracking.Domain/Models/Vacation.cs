using Dapper.FluentMap.Mapping;
using System;

namespace VacationTracking.Domain.Models
{
    public class Vacation : BaseModel
    {
        public Guid VacationId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ApproverId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
    public class VacationsMap : EntityMap<Vacation>
    {
        public VacationsMap()
        {
            Map(p => p.VacationId).ToColumn("vacation_id");
            Map(p => p.UserId).ToColumn("user_id");
            Map(p => p.ApproverId).ToColumn("approver_id");
            Map(p => p.LeaveTypeId).ToColumn("leave_type_id");
            Map(p => p.Status).ToColumn("vacation_status");
            Map(p => p.StartDate).ToColumn("start_date");
            Map(p => p.EndDate).ToColumn("end_date");
            Map(p => p.Reason).ToColumn("reason");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }

}
