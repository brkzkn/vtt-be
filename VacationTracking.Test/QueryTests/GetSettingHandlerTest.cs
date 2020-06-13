using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Queries.Setting;
using VacationTracking.Service.Queries.Setting;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;
using SettingsDb = VacationTracking.Domain.Models.Setting;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetSettingHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetCompanySettingListHandler> _logger;
        private readonly IMapper _mapper;

        public GetSettingHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetCompanySettingListHandler>();

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

        /// Test.1: Hiç bir settings tanımlı değilse default değerleri dön.
        [Fact]
        public async Task Should_ReturnDefaultSettings_When_DoesNotExistCustomSettings()
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

            IRepository<SettingsDb> repository = new Repository<SettingsDb>(_fixture.Context);

            var handler = new GetCompanySettingListHandler(repository, _mapper, _logger);

            var request = new GetCompanySettingListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("default_value_1", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("default_value_2", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.2: Company ile alakalı settings tanımlıysa ilgili tablodaki değerleri dön.
        [Fact]
        public async Task Should_ReturnCompanySettings_When_CompanyHasCustomSettingsValue()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.Company
            };
            entity.CompanySettings.Add(new Domain.Models.CompanySetting()
            {
                CompanyId = 1,
                SettingValue = "company_1_value"
            });
            entity.CompanySettings.Add(new Domain.Models.CompanySetting()
            {
                CompanyId = 2,
                SettingValue = "company_2_value"
            });
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.Company
            };
            _fixture.Context.Settings.Attach(entity);

            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> repository = new Repository<SettingsDb>(_fixture.Context);

            var handler = new GetCompanySettingListHandler(repository, _mapper, _logger);

            var request = new GetCompanySettingListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("company_1_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("default_value_2", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }

        /// Test.3: User ile alakalı settings tanımlıysa ilgili tablodaki değerleri dön.
        [Fact]
        public async Task Should_ReturnUserSettings_When_UserHasCustomSettingsValue()
        {
            // Arrange
            var entity = new SettingsDb()
            {
                SettingKey = "key_1",
                DefaultValue = "default_value_1",
                SettingType = Domain.Enums.SettingType.User
            };
            entity.UserSettings.Add(new Domain.Models.UserSetting()
            {
                UserId = 1,
                SettingValue = "user_1_value"
            });
            entity.UserSettings.Add(new Domain.Models.UserSetting()
            {
                UserId = 2,
                SettingValue = "user_2_value"
            });
            _fixture.Context.Settings.Attach(entity);

            entity = new SettingsDb()
            {
                SettingKey = "key_2",
                DefaultValue = "default_value_2",
                SettingType = Domain.Enums.SettingType.User
            };
            _fixture.Context.Settings.Attach(entity);

            _fixture.Context.SaveChanges();

            IRepository<SettingsDb> repository = new Repository<SettingsDb>(_fixture.Context);

            var handler = new GetUserSettingListHandler(repository, _mapper, _logger);

            var request = new GetUserSettingListQuery(userId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("user_1_value", result.SingleOrDefault(x => x.Key == "key_1").Value);
            Assert.Equal("default_value_2", result.SingleOrDefault(x => x.Key == "key_2").Value);
        }
    }
}
