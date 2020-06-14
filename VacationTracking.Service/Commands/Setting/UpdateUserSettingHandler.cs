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
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Setting;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using SettingDb = VacationTracking.Domain.Models.Setting;
using UserSettingDb = VacationTracking.Domain.Models.UserSetting;

namespace VacationTracking.Service.Commands.Setting
{
    public class UpdateUserSettingHandler : IRequestHandler<UpdateUserSettingCommand, IEnumerable<SettingsDto>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserSettingDb> _repository;
        private readonly IRepository<SettingDb> _settingRepository;

        public UpdateUserSettingHandler(IUnitOfWork unitOfWork,
                                           IRepository<UserSettingDb> repository,
                                           IRepository<SettingDb> settingRepository,
                                           ILogger<UpdateUserSettingHandler> logger,
                                           IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        public async Task<IEnumerable<SettingsDto>> Handle(UpdateUserSettingCommand request, CancellationToken cancellationToken)
        {
            var settings = await _settingRepository.Queryable()
                                                   .Where(x => x.SettingType == Domain.Enums.SettingType.User)
                                                   .ToListAsync();

            var userSettings = await _repository.Queryable()
                                                .Where(x => x.UserId == request.UserId)
                                                .ToListAsync();

            Dictionary<string, SettingsDto> result = settings.Select(x => new SettingsDto()
            {
                Key = x.SettingKey,
                SettingId = x.SettingId,
                Value = userSettings.Any(cs => cs.SettingId == x.SettingId) ?
                        userSettings.SingleOrDefault(cs => cs.SettingId == x.SettingId).SettingValue :
                        x.DefaultValue
            }).ToDictionary(x => x.Key, x => x);

            foreach (var setting in request.SettingsDtos)
            {
                var existSetting = settings.SingleOrDefault(x => x.SettingKey == setting.Key);
                if (existSetting == null)
                    throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Setting not found by key: {setting.Key}", 400);

                var companySetting = userSettings.SingleOrDefault(x => x.SettingId == existSetting.SettingId);
                if (companySetting == null)
                {
                    _repository.Insert(new UserSettingDb()
                    {
                        UserId = request.UserId,
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
