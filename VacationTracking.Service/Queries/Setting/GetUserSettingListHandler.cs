using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Setting;
using SettingDb = VacationTracking.Domain.Models.Setting;

namespace VacationTracking.Service.Queries.Setting
{
    public class GetUserSettingListHandler : IRequestHandler<GetUserSettingListQuery, IEnumerable<SettingsDto>>
    {
        private readonly IRepository<SettingDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserSettingListHandler(IRepository<SettingDb> repository, IMapper mapper, ILogger<GetCompanySettingListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<SettingsDto>> Handle(GetUserSettingListQuery request, CancellationToken cancellationToken)
        {
            var settings = await _repository.Queryable()
                                            .Where(x => x.SettingType == Domain.Enums.SettingType.User)
                                            .Select(x => new
                                            {
                                                Settings = x,
                                                UserSettings = x.UserSettings.Where(c => c.UserId == request.UserId)
                                            })
                                            .ToListAsync();

            List<SettingsDto> settingsDto = new List<SettingsDto>();

            if (settings != null)
            {
                _logger.LogInformation($"Got a request get user setting by UserId: {request.UserId}");
                foreach (var setting in settings)
                {
                    var companySettings = setting.UserSettings.FirstOrDefault(x => x.SettingId == setting.Settings.SettingId);
                    settingsDto.Add(new SettingsDto()
                    {
                        SettingId = setting.Settings.SettingId,
                        Key = setting.Settings.SettingKey,
                        Value = companySettings == null ? setting.Settings.DefaultValue : companySettings.SettingValue
                    });
                }
                return settingsDto;
            }

            _logger.LogInformation($"Settings not found by UserId: {request.UserId}");

            return settingsDto;
        }
    }
}
