using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;

namespace VacationTracking.Domain.Models
{
    public class Holiday :BaseModel
    {
        public Guid HolidayId { get; set; }
        public Guid CompanyId { get; set; }
        public string HolidayName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFullDay { get; set; }
        
        public List<Team> Teams { get; set; }
    }

    public class HolidaysMap : EntityMap<Holiday>
    {
        public HolidaysMap()
        {
            Map(p => p.HolidayId).ToColumn("holiday_id");
            Map(p => p.CompanyId).ToColumn("company_id");
            Map(p => p.HolidayName).ToColumn("name");
            Map(p => p.StartDate).ToColumn("start_date");
            Map(p => p.EndDate).ToColumn("end_date");
            Map(p => p.IsFullDay).ToColumn("is_full_day");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }
}
