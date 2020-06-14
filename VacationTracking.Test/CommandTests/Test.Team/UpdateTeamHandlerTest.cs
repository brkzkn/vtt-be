using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.Team;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class UpdateTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<UpdateTeamHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<UpdateTeamHandler>();

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
        public async Task Should_UpdateTeam_When_PassValidParameters()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = -1
            };
            team.TeamMembers.Add(new TeamMember()
            {
                IsApprover = true,
                IsMember = true,
                UserId = 1
            });
            _fixture.Context.Teams.Attach(team);
            await _fixture.Context.SaveChangesAsync();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new UpdateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new UpdateTeamCommand(companyId: 1,
                                                userId: 1,
                                                teamId: 1,
                                                "Test Team - 1",
                                                members: new List<int>() { 1, 2 },
                                                approvers: new List<int>() { 1 });

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("Test Team - 1", result.TeamName);
            Assert.Equal(2, result.TeamMembers.Count(x => x.IsMember));
            Assert.Equal(1, result.TeamMembers.Count(x => x.IsApprover));
            Assert.Equal(1, result.ModifiedBy);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidUserId()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = -1
            };
            team.TeamMembers.Add(new TeamMember()
            {
                IsApprover = true,
                IsMember = true,
                UserId = 1
            });
            _fixture.Context.Teams.Attach(team);
            await _fixture.Context.SaveChangesAsync();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new UpdateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new UpdateTeamCommand(companyId: 1,
                                                userId: 1,
                                                teamId: 1,
                                                "Test Team - 1",
                                                members: new List<int>() { 1, -22 },
                                                approvers: new List<int>() { 1 });

            // Act
            var tcs = new CancellationToken();

            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            Assert.Equal(ExceptionMessages.InvalidUserId, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_UpdateTeamMember_When_PassMemberUserIds()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = -1
            };
            team.TeamMembers.Add(new TeamMember()
            {
                IsApprover = true,
                IsMember = true,
                UserId = 1
            });
            _fixture.Context.Teams.Attach(team);
            await _fixture.Context.SaveChangesAsync();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new UpdateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new UpdateTeamCommand(companyId: 1,
                                                userId: 1,
                                                teamId: 1,
                                                "Test Team - 1",
                                                members: new List<int>() { 2 },
                                                approvers: new List<int>() { 2 });

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal(1, result.TeamMembers.Count(x => x.IsMember));
            Assert.Equal(1, result.TeamMembers.Count(x => x.IsApprover));
            Assert.DoesNotContain(result.TeamMembers, x => x.UserId == 1);
            Assert.Equal(1, result.ModifiedBy);
        }

        [Fact]
        public async Task Should_ThrowException_When_ExistingTeamName()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = -1
            };
            team.TeamMembers.Add(new TeamMember()
            {
                IsApprover = true,
                IsMember = true,
                UserId = 1
            });
            _fixture.Context.Teams.Attach(team);
            await _fixture.Context.SaveChangesAsync();

            team = new Team()
            {
                TeamName = "Test Team - 1",
                CompanyId = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = -1
            };
            team.TeamMembers.Add(new TeamMember()
            {
                IsApprover = true,
                IsMember = true,
                UserId = 2
            });
            _fixture.Context.Teams.Attach(team);
            await _fixture.Context.SaveChangesAsync();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new UpdateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new UpdateTeamCommand(companyId: 1,
                                                userId: 1,
                                                teamId: 1,
                                                "Test Team - 1",
                                                members: new List<int>() { 1, -22 },
                                                approvers: new List<int>() { 1 });

            // Act
            var tcs = new CancellationToken();

            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            Assert.Equal(ExceptionMessages.TeamNameAlreadyExist, exception.Message);
            Assert.Equal(400, exception.Code);
        }
    }
}
