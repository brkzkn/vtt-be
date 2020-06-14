using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.Holiday;
using VacationTracking.Service.Queries.Holiday;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetHolidayListHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetHolidayListHandler> _logger;
        private readonly IMapper _mapper;
        public GetHolidayListHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetHolidayListHandler>();

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

        [Fact]
        public async Task Should_ReturnLeaveTypeList_When_PassValidCompanyId()
        {
            // Arrange
            var holiday = new Holiday()
            {
                HolidayId = 1,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                Name = "Holiday Test - 1"
            };
            _fixture.Context.Holidays.Add(holiday);
            holiday = new Holiday()
            {
                HolidayId = 2,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                Name = "Holiday Test - 2"
            };
            _fixture.Context.Holidays.Add(holiday);
            _fixture.Context.SaveChanges();

            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);

            var handler = new GetHolidayListHandler(repository, _mapper, _logger);

            var queryRequest = new GetHolidayListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassValidCompanyId()
        {
            // Arrange
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);

            var handler = new GetHolidayListHandler(repository, _mapper, _logger);

            var queryRequest = new GetHolidayListQuery(companyId: 3);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassInvalidCompanyId()
        {
            // Arrange
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);

            var handler = new GetHolidayListHandler(repository, _mapper, _logger);

            var queryRequest = new GetHolidayListQuery(companyId: -1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.Empty(result);
        }
    }
}
