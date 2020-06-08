using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.Holiday;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class DeleteHolidayHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<DeleteHolidayHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteHolidayHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<DeleteHolidayHandler>();

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
        public async Task Should_DeleteHoliday_When_PassHolidayId()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
                Name = "Test Holiday",
                IsFullDay = true,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1
            };
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 2
            });

            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new DeleteHolidayHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteHolidayCommand(companyId: 1, holidayId: 1);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team1 = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.TeamId == 1);
            var team2 = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.TeamId == 2);

            // Assert
            Assert.DoesNotContain(team1.HolidaysTeam, x => x.HolidayId == 1);
            Assert.DoesNotContain(team2.HolidaysTeam, x => x.HolidayId == 1);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidHolidayId()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new DeleteHolidayHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteHolidayCommand(companyId: 1, holidayId: -1);

            // Act 
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }
    }
}
