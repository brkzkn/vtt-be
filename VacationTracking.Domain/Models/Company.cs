using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace VacationTracking.Domain.Models
{
    public class Company : BaseModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
    }

    public class CompaniesMap : EntityMap<Company>
    {
        public CompaniesMap()
        {
            Map(p => p.CompanyId).ToColumn("company_id");
            Map(p => p.CompanyName).ToColumn("company_name");
            Map(p => p.Address1).ToColumn("address_1");
            Map(p => p.Address2).ToColumn("address_2");
            Map(p => p.Country).ToColumn("country");
            Map(p => p.Logo).ToColumn("logo");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }
}
