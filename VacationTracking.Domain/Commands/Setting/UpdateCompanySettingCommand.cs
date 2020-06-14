using MediatR;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Setting
{
    public class UpdateCompanySettingCommand : IRequest<IEnumerable<SettingsDto>>
    {
        public UpdateCompanySettingCommand(int companyId, IEnumerable<SettingsDto> settingsDtos)
        {
            CompanyId = companyId;
            SettingsDtos = new HashSet<SettingsDto>();
            SettingsDtos = settingsDtos;
        }

        public IEnumerable<SettingsDto> SettingsDtos { get; }
        public int CompanyId { get; }
    }
}