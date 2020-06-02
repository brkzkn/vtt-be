using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.Holiday;
using VacationTracking.Service.Queries.Holiday;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetHolidayHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetHolidayHandler> _logger;
        private readonly IMapper _mapper;
        public GetHolidayHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetHolidayHandler>();

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
        public async Task Should_ReturnHoliday_When_PassValidHolidayId()
        {
            // Arrange
            var holiday = new Holiday()
            {
                CompanyId = 1,
                HolidayId = 1,
                Name = "Holiday Test"
            };
            _fixture.Context.Holidays.Add(holiday);
            _fixture.Context.SaveChanges();

            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);

            var handler = new GetHolidayHandler(repository, _mapper, _logger);

            var query = new GetHolidayQuery(companyId: 1, holidayId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(query, tcs);

            // Assert
            Assert.Equal(1, result.HolidayId);
            Assert.Equal(1, result.CompanyId);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidHolidayId()
        {
            // Arrange
            var holiday = new Holiday()
            {
                CompanyId = 1,
                HolidayId = 1,
                Name = "Holiday Test"
            };
            _fixture.Context.Holidays.Add(holiday);
            _fixture.Context.SaveChanges();

            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            var handler = new GetHolidayHandler(repository, _mapper, _logger);

            var query = new GetHolidayQuery(companyId: 1, holidayId: -1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(query, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_HolidayIdDoesNotBelongsToCompany()
        {
            var holiday = new Holiday()
            {
                CompanyId = 1,
                HolidayId = 1,
                Name = "Holiday Test"
            };
            _fixture.Context.Holidays.Add(holiday);
            _fixture.Context.SaveChanges();
            // Arrange
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            var handler = new GetHolidayHandler(repository, _mapper, _logger);

            var query = new GetHolidayQuery(companyId: 2, holidayId: 1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(query, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }
    }
}
