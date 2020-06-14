using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Setting
{
    public class GetCompanySettingListQuery : QueryBase<IEnumerable<SettingsDto>>
    {
        public GetCompanySettingListQuery(int companyId)
        {
            CompanyId = companyId;
        }

        [Required]
        public int CompanyId { get; }
    }
}
