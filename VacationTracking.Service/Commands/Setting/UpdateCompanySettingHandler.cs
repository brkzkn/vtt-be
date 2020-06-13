using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Setting;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using CompanySettingDb = VacationTracking.Domain.Models.CompanySetting;
using SettingDb = VacationTracking.Domain.Models.Setting;

namespace VacationTracking.Service.Commands.Setting
{
    public class UpdateCompanySettingHandler : IRequestHandler<UpdateCompanySettingCommand, IEnumerable<SettingsDto>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CompanySettingDb> _repository;
        private readonly IRepository<SettingDb> _settingRepository;

        public UpdateCompanySettingHandler(IUnitOfWork unitOfWork,
                                           IRepository<CompanySettingDb> repository,
                                           IRepository<SettingDb> settingRepository,
                                           ILogger<UpdateCompanySettingHandler> logger,
                                           IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        public async Task<IEnumerable<SettingsDto>> Handle(UpdateCompanySettingCommand request, CancellationToken cancellationToken)
        {
            var settings = await _settingRepository.Queryable()
                                                   .Where(x => x.SettingType == Domain.Enums.SettingType.Company)
                                                   .ToListAsync();

            var companySettings = await _repository.Queryable()
                                                   .Where(x => x.CompanyId == request.CompanyId)
                                                   .ToListAsync();

            Dictionary<string, SettingsDto> result = settings.Select(x => new SettingsDto()
            {
                Key = x.SettingKey,
                SettingId = x.SettingId,
                Value = companySettings.Any(cs => cs.SettingId == x.SettingId) ? 
                        companySettings.SingleOrDefault(cs => cs.SettingId == x.SettingId).SettingValue :
                        x.DefaultValue
            }).ToDictionary(x => x.Key, x => x);

            foreach (var setting in request.SettingsDtos)
            {
                var existSetting = settings.SingleOrDefault(x => x.SettingKey == setting.Key);
                if (existSetting == null)
                    throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Setting not found by key: {setting.Key}", 400);

                var companySetting = companySettings.SingleOrDefault(x => x.SettingId == existSetting.SettingId);
                if (companySetting == null)
                {
                    _repository.Insert(new CompanySettingDb()
                    {
                        CompanyId = request.CompanyId,
                        SettingId = existSetting.SettingId,
                        SettingValue = setting.Value
                    });
                }
                else
                {
                    companySetting.SettingValue = setting.Value;
                    _repository.Update(companySetting);
                }

                result[setting.Key].Value = setting.Value;
            }

            var affectedRow = await _unitOfWork.SaveChangesAsync();

            return result.Values.AsEnumerable();
        }
    }
}
