using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Enums;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.Team;
using VacationTracking.Service.Queries.Team;
using VacationTracking.Service.Validation.Queries.Team;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetTeamHandler> _logger;
        private readonly IMapper _mapper;
        public GetTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetTeamHandler>();

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
        public async Task Should_ReturnTeamObject_When_PassValidTeamId()
        {
            // Arrange
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);

            var getTeamHandler = new GetTeamHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamQuery(teamId: 1, companyId: 1, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var team = await getTeamHandler.Handle(queryRequest, tcs);

            // Assert
            Assert.Equal(1, team.TeamId);
            Assert.Equal(1, team.CompanyId);
            Assert.NotNull(team.TeamMembers);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidTeamId()
        {
            // Arrange
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var getTeamHandler = new GetTeamHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamQuery(teamId: -10, companyId: 1, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await getTeamHandler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_TeamIdIsNotBelongsToCompany()
        {
            // Arrange
            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var getTeamHandler = new GetTeamHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamQuery(teamId: 1, companyId: 2, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await getTeamHandler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_UserDoesNotHavePermission()
        {
            // Arrange
            var user = new User
            {
                UserId = 5,
                CompanyId = 1,
                Email = "test.user@vt.com",
                CreatedAt = DateTime.Now,
                EmployeeSince = DateTime.UtcNow.AddYears(-3).Date,
                FullName = "Burak Ozkan",
                Status = UserStatus.Active,
                AccountType = AccountType.Member
            };
            _fixture.Context.Users.Add(user);
            _fixture.Context.SaveChanges();

            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var getTeamHandler = new GetTeamHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamQuery(teamId: 1, companyId: 2, userId: 5);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await getTeamHandler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
        }

        [Fact]
        public async Task Should_ReturnOnlyActiveMember_When_TeamHasDisabledMember()
        {
            // Arrange
            var query = new GetTeamQuery(teamId: 1, companyId: -5, userId: 5);
            var user = new User()
            {
                UserName = "inactive_user",
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                AccountType = AccountType.Member,
                Status = UserStatus.Disabled,
                FullName = "Test User",
                Email = "inactive.user@vtt.com",
                UserId = 10
            };
            var teamMember = new TeamMember()
            {
                TeamId = 1,
                UserId = 10,
                IsApprover = true,
                IsMember = false
            };

            _fixture.Context.Users.Add(user);
            _fixture.Context.TeamMembers.Add(teamMember);
            _fixture.Context.SaveChanges();

            IRepository<Team> teamRepository = new Repository<Team>(_fixture.Context);
            var getTeamHandler = new GetTeamHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamQuery(teamId: 1, companyId: 1, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var team = await getTeamHandler.Handle(queryRequest, tcs);

            // Assert
            Assert.All(team.TeamMembers, item => Assert.NotEqual(10, item.UserId));
        }
    }
}
