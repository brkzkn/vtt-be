using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Setting;
using SettingDb = VacationTracking.Domain.Models.Setting;

namespace VacationTracking.Service.Queries.Setting
{
    public class GetCompanySettingListHandler : IRequestHandler<GetCompanySettingListQuery, IEnumerable<SettingsDto>>
    {
        private readonly IRepository<SettingDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetCompanySettingListHandler(IRepository<SettingDb> repository, IMapper mapper, ILogger<GetCompanySettingListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<SettingsDto>> Handle(GetCompanySettingListQuery request, CancellationToken cancellationToken)
        {
            var settings = await _repository.Queryable()
                                            .Where(x => x.SettingType == Domain.Enums.SettingType.Company)
                                            .Select(x => new
                                            {
                                                Settings = x,
                                                CompanySettings = x.CompanySettings.Where(c => c.CompanyId == request.CompanyId)
                                            })                                            
                                            .ToListAsync();

            List<SettingsDto> settingsDto = new List<SettingsDto>();

            if (settings != null)
            {
                _logger.LogInformation($"Got a request get company setting by CompanyId: {request.CompanyId}");
                foreach (var setting in settings)
                {
                    var companySettings = setting.CompanySettings.FirstOrDefault(x => x.SettingId == setting.Settings.SettingId);
                    settingsDto.Add(new SettingsDto()
                    {
                        SettingId = setting.Settings.SettingId,
                        Key = setting.Settings.SettingKey,
                        Value = companySettings == null ? setting.Settings.DefaultValue : companySettings.SettingValue
                    });
                }
                return settingsDto;
            }

            _logger.LogInformation($"Settings not found by CompanyId: {request.CompanyId}");

            return settingsDto;
        }
    }
}
