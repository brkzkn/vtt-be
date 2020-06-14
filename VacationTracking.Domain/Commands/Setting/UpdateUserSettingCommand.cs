using MediatR;
using System.Collections.Generic;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Setting
{
    public class UpdateUserSettingCommand : IRequest<IEnumerable<SettingsDto>>
    {
        public UpdateUserSettingCommand(int userId, IEnumerable<SettingsDto> settingsDtos)
        {
            UserId = userId;
            SettingsDtos = settingsDtos;
        }

        public int UserId { get; }
        public IEnumerable<SettingsDto> SettingsDtos { get; }

    }
}
