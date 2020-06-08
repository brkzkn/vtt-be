using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using System;
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
    public class DeleteTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<DeleteTeamHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<DeleteTeamHandler>();

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
                _fixture.Context.SaveChanges();
            });
        }

        [Fact]
        public async Task Should_DeleteTeamWithMember_When_PassTeamId()
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
            IRepository<TeamMember> teamMemberRepository = new Repository<TeamMember>(_fixture.Context);

            var handler = new DeleteTeamHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteTeamCommand(companyId: 1, teamId: 1);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.True(result);
            Assert.False(teamMemberRepository.Queryable().Any(x => x.TeamId == 1));
        }

        [Fact]
        public async Task Should_ThrowException_When_PassTeamIdForDefaultTeam()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 1,
                IsDefault = true,
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
            IRepository<TeamMember> teamMemberRepository = new Repository<TeamMember>(_fixture.Context);

            var handler = new DeleteTeamHandler(unitOfWork, repository, _logger, _mapper);
            var request = new DeleteTeamCommand(companyId: 1, teamId: 1);

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.DefaultTeamCannotDelete, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassTeamIdForDifferentCompany()
        {
            // Arrange
            var team = new Team()
            {
                TeamName = "Test Team",
                CompanyId = 2,
                IsDefault = true,
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
            IRepository<TeamMember> teamMemberRepository = new Repository<TeamMember>(_fixture.Context);

            var handler = new DeleteTeamHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteTeamCommand(companyId: 1, teamId: 1);

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(404, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidTeamId()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<TeamMember> teamMemberRepository = new Repository<TeamMember>(_fixture.Context);

            var handler = new DeleteTeamHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteTeamCommand(companyId: 1, teamId: -1);

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(404, exception.Code);
        }
    }
}
