using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Setting;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Service.Commands.Setting;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;
using CompanySettingsDb = VacationTracking.Domain.Models.CompanySetting;
using UserSettingsDb = VacationTracking.Domain.Models.UserSetting;
using SettingsDb = VacationTracking.Domain.Models.Setting;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class UpdateSettingHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<UpdateCompanySettingHandler> _companyLogger;
        private readonly NullLogger<UpdateUserSettingHandler> _userLogger;
        private readonly IMapper _mapper;

        public UpdateSettingHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _companyLogger = new NullLogger<UpdateCompanySettingHandler>();
            _userLogger = new NullLogger<UpdateUserSettingHandler>();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(Service.Mapper.AutoMapping));
            });
            _mapper = mockMapper.CreateMapper();

            _fixture.Initialize(true, () =>
            {
                _fixture.Context.Companies.AddRange(Seed.Companies());
                _fixture.Context.Users.AddRange(Seed.Users());
                _fixture.Context.Teams.AddRange(Seed.Teams());
                _fixture.Context.TeamMembers.AddRange(Seed.TeamMembers());
                _fixture.Context.SaveChanges();
            });
        }

        /// Test.1: Company için bir settings değeri kaydetme.
        [Fact]
        public async Task Should_UpdateCompanySetting_When_PassValidParameters()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.Company
            };
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.Company
            };
            _fixture.Context.Settings.Attach(entity);

            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> settingRepository = new Repository<SettingsDb>(_fixture.Context);
            IRepository<CompanySettingsDb> repository = new Repository<CompanySettingsDb>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new UpdateCompanySettingHandler(unitOfWork, repository, settingRepository, _companyLogger, _mapper);

            var dtos = new List<SettingsDto>();
            dtos.Add(new SettingsDto()
            {
                Key = "key_1",
                Value = "company1_key1_value"
            });
            dtos.Add(new SettingsDto()
            {
                Key = "key_2",
                Value = "company1_key2_value"
            });

            var request = new UpdateCompanySettingCommand(companyId: 1, dtos);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("company1_key1_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("company1_key2_value", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.2: User için bir settings değeri kaydetme.
        [Fact]
        public async Task Should_UpdateUserSetting_When_PassValidParameters()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.User
            };
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.User
            };
            _fixture.Context.Settings.Attach(entity);

            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> settingRepository = new Repository<SettingsDb>(_fixture.Context);
            IRepository<UserSettingsDb> repository = new Repository<UserSettingsDb>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new UpdateUserSettingHandler(unitOfWork, repository, settingRepository, _userLogger, _mapper);

            var dtos = new List<SettingsDto>();
            dtos.Add(new SettingsDto()
            {
                Key = "key_1",
                Value = "user1_key1_value"
            });
            dtos.Add(new SettingsDto()
            {
                Key = "key_2",
                Value = "user1_key2_value"
            });

            var request = new UpdateUserSettingCommand(userId: 1, dtos);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("user1_key1_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("user1_key2_value", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.3: Company için var olan bir setting değerini güncelleme.
        [Fact]
        public async Task Should_UpdateExistCompanySetting_When_PassValidParameters()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.Company
            };
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 1,
                SettingValue = "company1_key1_value"
            });
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 2,
                SettingValue = "company2_key1_value"
            });
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.Company
            };
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 1,
                SettingValue = "company1_key2_value"
            });
            _fixture.Context.Settings.Attach(entity);
            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> settingRepository = new Repository<SettingsDb>(_fixture.Context);
            IRepository<CompanySettingsDb> repository = new Repository<CompanySettingsDb>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new UpdateCompanySettingHandler(unitOfWork, repository, settingRepository, _companyLogger, _mapper);

            var dtos = new List<SettingsDto>();
            dtos.Add(new SettingsDto()
            {
                Key = "key_1",
                Value = "company1_key1_updated_value"
            });

            var request = new UpdateCompanySettingCommand(companyId: 1, dtos);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("company1_key1_updated_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("company1_key2_value", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.4: User için var olan bir setting değerini güncelleme.
        [Fact]
        public async Task Should_UpdateExistUserSetting_When_PassValidParameters()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.User
            };
            entity.UserSettings.Add(new UserSettingsDb()
            {
                UserId = 1,
                SettingValue = "user1_key1_value"
            });
            entity.UserSettings.Add(new UserSettingsDb()
            {
                UserId= 2,
                SettingValue = "user2_key1_value"
            });
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.User
            };
            entity.UserSettings.Add(new UserSettingsDb()
            {
                UserId= 1,
                SettingValue = "user1_key2_value"
            });
            _fixture.Context.Settings.Attach(entity);
            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> settingRepository = new Repository<SettingsDb>(_fixture.Context);
            IRepository<UserSettingsDb> repository = new Repository<UserSettingsDb>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new UpdateUserSettingHandler(unitOfWork, repository, settingRepository, _userLogger, _mapper);

            var dtos = new List<SettingsDto>();
            dtos.Add(new SettingsDto()
            {
                Key = "key_1",
                Value = "user1_key1_updated_value"
            });

            var request = new UpdateUserSettingCommand(userId: 1, dtos);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("user1_key1_updated_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("user1_key2_value", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.5: Setting tablosunda olmayan bir değer için güncelleme.
        [Fact]
        public async Task Should_ThrowException_When_PassInvalidSettingKey()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.Company
            };
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 1,
                SettingValue = "company1_key1_value"
            });
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 2,
                SettingValue = "company2_key1_value"
            });
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.Company
            };
            entity.CompanySettings.Add(new CompanySettingsDb()
            {
                CompanyId = 1,
                SettingValue = "company1_key2_value"
            });
            _fixture.Context.Settings.Attach(entity);
            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> settingRepository = new Repository<SettingsDb>(_fixture.Context);
            IRepository<CompanySettingsDb> repository = new Repository<CompanySettingsDb>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new UpdateCompanySettingHandler(unitOfWork, repository, settingRepository, _companyLogger, _mapper);

            var dtos = new List<SettingsDto>();
            dtos.Add(new SettingsDto()
            {
                Key = "key_5",
                Value = "company1_key5_updated_value"
            });

            var request = new UpdateCompanySettingCommand(companyId: 1, dtos);
            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(400, exception.Code);
        }
    }
}
