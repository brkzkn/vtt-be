using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using VacationTracking.Data;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using VacationTracking.Test.QueryTests;
using Xunit;

namespace VacationTracking.Test.CommandTests.Test.Setting
{
    [Collection(nameof(VacationTrackingContext))]
    public class UpdateSettingHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetSettingHandlerTest> _logger;
        private readonly IMapper _mapper;

        public UpdateSettingHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetSettingHandlerTest>();

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
        /// Test.2: User için bir settings değeri kaydetme.
        /// Test.3: Company için var olan bir setting değerini güncelleme.
        /// Test.4: User için var olan bir setting değerini güncelleme.
        /// Test.5: Setting tablosunda olmayan bir değer için güncelleme.
    }
}
