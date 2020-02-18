using Dapper.FluentMap.Mapping;
using System;

namespace VacationTracking.Domain.Models
{
    public class User : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime EmployeeSince { get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; }
    }

    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            Map(p => p.UserId).ToColumn("user_id");
            Map(p => p.CompanyId).ToColumn("company_id");
            Map(p => p.FullName).ToColumn("full_name");
            Map(p => p.Email).ToColumn("email");
            Map(p => p.EmployeeSince).ToColumn("employee_since");
            Map(p => p.Status).ToColumn("status");
            Map(p => p.AccountType).ToColumn("account_type");
            Map(p => p.CreatedAt).ToColumn("created_at");
            Map(p => p.CreatedBy).ToColumn("created_by");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
            Map(p => p.UpdatedBy).ToColumn("updated_by");
        }
    }
}
