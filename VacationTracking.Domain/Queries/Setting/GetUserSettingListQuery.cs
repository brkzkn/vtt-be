using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.Setting
{
    public class GetUserSettingListQuery : QueryBase<IEnumerable<SettingsDto>>
    {
        public GetUserSettingListQuery(int userId)
        {
            UserId = userId;
        }

        [Required]
        public int UserId { get; }
    }
}
