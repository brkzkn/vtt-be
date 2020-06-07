using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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
using VacationTracking.Service.Validation.Commands.Holiday;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class CreateHolidayHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<CreateHolidayHandler> _logger;
        private readonly IMapper _mapper;
        public CreateHolidayHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<CreateHolidayHandler>();

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
        public async Task Should_CreateHoliday_When_PassValidParameters()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   new List<int>() { 1 },
                                                   DateTime.Now.Date,
                                                   DateTime.Now.Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal(1, result.CreatedBy);
            Assert.Equal(1, result.HolidayId);

        }

        [Fact]
        public async Task Should_CreateHolidayForTeam_When_PassTeamId()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   new List<int> { 1 },
                                                   DateTime.Now.Date,
                                                   DateTime.Now.Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team = teamRepository.Queryable().SingleOrDefault(x => x.CompanyId == 1 && x.TeamId == 1);

            // Assert
            Assert.Equal(1, result.CreatedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.Contains(team.HolidaysTeam, x => x.HolidayId == 1);
        }

        [Fact]
        public async Task Should_CreateHolidayForTeam_When_PassMultipleTeamId()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   new List<int> { 1, 2 },
                                                   DateTime.Now.Date,
                                                   DateTime.Now.Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team1 = teamRepository.Queryable().SingleOrDefault(x => x.CompanyId == 1 && x.TeamId == 1);
            var team2 = teamRepository.Queryable().SingleOrDefault(x => x.CompanyId == 1 && x.TeamId == 2);

            // Assert
            Assert.Equal(1, result.CreatedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.Contains(team1.HolidaysTeam, x => x.HolidayId == 1);
            Assert.Contains(team2.HolidaysTeam, x => x.HolidayId == 1);
        }

        [Fact]
        public async Task Should_CreateHolidayForAllTeams_When_SetIsForAllTeamsParameter()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   null,
                                                   DateTime.Now.Date,
                                                   DateTime.Now.Date,
                                                   "Test Holiday",
                                                   isForAllTeams: true,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team = teamRepository.Queryable().Where(x => x.CompanyId == 1).ToList();

            // Assert
            Assert.Equal(1, result.CreatedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.All<Team>(team, x =>
            {
                Assert.Contains(x.HolidaysTeam, x => x.HolidayId == 1);
            });
        }

        [Fact]
        public async Task Should_ThrowException_When_CreatingHolidayForAllTeams()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(-1).Date,
                EndDate = DateTime.Now.AddDays(-1).Date,
                Name = "Mock Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.Holidays.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   null,
                                                   DateTime.Now.AddDays(-1).Date,
                                                   DateTime.Now.AddDays(-1).Date,
                                                   "Test Holiday",
                                                   isForAllTeams: true,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();

            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(request, tcs);
            });
            Assert.Equal(ExceptionMessages.HolidayAlreadyExistForSameDate, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassExistingStartDateForOneTeam()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(-1).Date,
                EndDate = DateTime.Now.AddDays(-1).Date,
                Name = "Mock Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   new List<int> { 1 },
                                                   DateTime.Now.AddDays(-1).Date,
                                                   DateTime.Now.AddDays(-1).Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();

            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(request, tcs);
            });
            Assert.Equal(ExceptionMessages.HolidayAlreadyExistForSameDate, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_CreateHoliday_When_PassNotExistingStartDateForTeam()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(-1).Date,
                EndDate = DateTime.Now.AddDays(-1).Date,
                Name = "Mock Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   new List<int> { 2 },
                                                   DateTime.Now.AddDays(-1).Date,
                                                   DateTime.Now.AddDays(-1).Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team = teamRepository.Queryable().SingleOrDefault(x => x.TeamId == 2);

            // Assert
            Assert.Equal(1, result.CreatedBy);
            Assert.Equal("Test Holiday", result.Name);
            Assert.Contains(team.HolidaysTeam, x => x.HolidayId == result.HolidayId);
        }

        [Fact]
        public async Task Should_ThrowException_When_InterceptDate_v1()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(-1).Date,
                EndDate = DateTime.Now.AddDays(-1).Date,
                Name = "Mock Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   teams: new List<int> { 1 },
                                                   startDate: DateTime.Now.AddDays(-2).Date,
                                                   endDate: DateTime.Now.AddDays(1).Date,
                                                   name: "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act & Assert
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(request, tcs);
            });
            Assert.Equal(ExceptionMessages.HolidayAlreadyExistForSameDate, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_InterceptDate_v2()
        {
            // Arrange
            var entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(-2).Date,
                EndDate = DateTime.Now.AddDays(1).Date,
                Name = "Mock Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   teams: new List<int> { 1 },
                                                   startDate: DateTime.Now.AddDays(-1).Date,
                                                   endDate: DateTime.Now.AddDays(-1).Date,
                                                   name: "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act & Assert
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(request, tcs);
            });
            Assert.Equal(ExceptionMessages.HolidayAlreadyExistForSameDate, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_StartDateLessThanEndDate()
        {
            HolidayCommandValidator validator = new HolidayCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   null,
                                                   endDate: DateTime.Now.Date,
                                                   startDate: DateTime.Now.AddDays(-1).Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_PassEmptyListForIsFullTeamFalse()
        {
            HolidayCommandValidator validator = new HolidayCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new CreateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new CreateHolidayCommand(companyId: 1,
                                                   userId: 1,
                                                   null,
                                                   endDate: DateTime.Now.Date,
                                                   startDate: DateTime.Now.Date,
                                                   "Test Holiday",
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }
    }
}
