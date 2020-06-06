using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class UpdateHolidayHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<UpdateHolidayHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateHolidayHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<UpdateHolidayHandler>();

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

        /// <summary>
        /// Test to update startDate, endDate, name and isFullDay property for same teamId
        /// </summary>
        [Fact]
        public async Task Should_UpdateHoliday_When_PassValidParameters()
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
            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Updated Holiday",
                                                   DateTime.Now.AddDays(-1).Date,
                                                   DateTime.Now.AddDays(1).Date,
                                                   new List<int>() { 1 },
                                                   isForAllTeams: true,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal(DateTime.Now.AddDays(-1).Date, result.StartDate);
            Assert.Equal(DateTime.Now.AddDays(1).Date, result.EndDate);
            Assert.Equal("Updated Holiday", result.Name);
            Assert.True(result.IsFullDay);
        }

        /// <summary>
        /// Test to update teamIds of holiday for one team to one team
        /// </summary>
        [Fact]
        public async Task Should_UpdateHolidayForTeam_When_ChangeTeamId()
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
            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Updated Holiday",
                                                   startDate: DateTime.Now.Date,
                                                   endDate: DateTime.Now.Date,
                                                   new List<int> { 2 },
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == 1 && x.TeamId == 2);

            // Assert
            Assert.Equal(1, result.ModifiedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.Contains(team.HolidaysTeam, x => x.HolidayId == 1);
        }

        /// <summary>
        /// Test to update teamIds of holiday for one team to one team
        /// </summary>
        [Fact]
        public async Task Should_RemoveHolidayForTeam_When_ChangeTeamId()
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
            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Updated Holiday",
                                                   startDate: DateTime.Now.Date,
                                                   endDate: DateTime.Now.Date,
                                                   new List<int> { 2 },
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == 1 && x.TeamId == 1);

            // Assert
            Assert.Equal(1, result.ModifiedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.DoesNotContain(team.HolidaysTeam, x => x.HolidayId == 1);
        }

        /// <summary>
        /// Test to update teamIds of holiday for one team to one team
        /// </summary>
        [Fact]
        public async Task Should_KeepHolidayForExistingTeam_When_ChangeTeamId()
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
            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Updated Holiday",
                                                   startDate: DateTime.Now.Date,
                                                   endDate: DateTime.Now.Date,
                                                   new List<int> { 1, 2 },
                                                   isForAllTeams: false,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team1 = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == 1 && x.TeamId == 1);
            var team2 = await teamRepository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == 1 && x.TeamId == 2);

            // Assert
            Assert.Equal(1, result.ModifiedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.Contains(team1.HolidaysTeam, x => x.HolidayId == 1);
            Assert.Contains(team2.HolidaysTeam, x => x.HolidayId == 1);
        }

        /// <summary>
        /// Test to update teamId of holiday for one team to multiple team
        /// </summary>
        [Fact]
        public async Task Should_UpdateHolidayForMultipleTeam_When_ChangeTeamId()
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
            _fixture.Context.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Updated Holiday",
                                                   startDate: DateTime.Now.Date,
                                                   endDate: DateTime.Now.Date,
                                                   teams: null,
                                                   isForAllTeams: true,
                                                   isFullDay: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var team1 = teamRepository.Queryable().SingleOrDefault(x => x.CompanyId == 1 && x.TeamId == 1);
            var team2 = teamRepository.Queryable().SingleOrDefault(x => x.CompanyId == 1 && x.TeamId == 2);

            // Assert
            Assert.Equal(1, result.ModifiedBy);
            Assert.Equal(1, result.HolidayId);
            Assert.Contains(team1.HolidaysTeam, x => x.HolidayId == 1);
            Assert.Contains(team2.HolidaysTeam, x => x.HolidayId == 1);
        }

        /// <summary>
        /// Test to update existingStartDate for team
        /// </summary>
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
                Name = "Mock-1 Holiday"
            };
            entity.HolidayTeam = new List<HolidayTeam>();
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(1).Date,
                EndDate = DateTime.Now.AddDays(1).Date,
                Name = "Mock-2 Holiday"
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

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Test Holiday",
                                                   startDate: DateTime.Now.AddDays(1).Date,
                                                   endDate: DateTime.Now.AddDays(1).Date,
                                                   teams: new List<int>() { 1 },
                                                   isForAllTeams: false,
                                                   isFullDay: false);

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

        /// <summary>
        /// Test to update existingEndDate for team
        /// </summary>
        [Fact]
        public async Task Should_ThrowException_When_ChangeExistingStartDate()
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
                Name = "Mock-1 Holiday"
            };
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });

            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            entity = new Holiday()
            {
                CompanyId = 1,
                IsFullDay = true,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                StartDate = DateTime.Now.AddDays(1).Date,
                EndDate = DateTime.Now.AddDays(1).Date,
                Name = "Mock-2 Holiday"
            };
            entity.HolidayTeam.Add(new HolidayTeam()
            {
                TeamId = 1
            });
            _fixture.Context.Holidays.Attach(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   name: "Test Holiday",
                                                   startDate: DateTime.Now.AddDays(1).Date,
                                                   endDate: DateTime.Now.AddDays(1).Date,
                                                   teams: null,
                                                   isForAllTeams: true,
                                                   isFullDay: false);

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
        public async Task Should_ThrowException_When_StartDateLessThanEndDate()
        {
            UpdateHolidayCommandValidator validator = new UpdateHolidayCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                   holidayId: 1,
                                                   userId: 1,
                                                   "Test Holiday",
                                                   startDate: DateTime.Now.AddDays(-1).Date,
                                                   endDate: DateTime.Now.Date,
                                                   teams: null,
                                                   isForAllTeams: true,
                                                   isFullDay: false);

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_PassEmptyListForIsFullTeamFalse()
        {
            UpdateHolidayCommandValidator validator = new UpdateHolidayCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Holiday> repository = new Repository<Holiday>(_fixture.Context);
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var handler = new UpdateHolidayHandler(unitOfWork, repository, teamRepository, _logger, _mapper);

            var request = new UpdateHolidayCommand(companyId: 1,
                                                               holidayId: 1,
                                                               userId: 1,
                                                               "Test Holiday",
                                                               startDate: DateTime.Now.AddDays(-1).Date,
                                                               endDate: DateTime.Now.Date,
                                                               teams: null,
                                                               isForAllTeams: false,
                                                               isFullDay: false);

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }
    }
}
