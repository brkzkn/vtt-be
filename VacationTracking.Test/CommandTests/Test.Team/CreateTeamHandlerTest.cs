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
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.Team;
using VacationTracking.Service.Validation.Commands.Team;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class CreateTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<CreateTeamHandler> _logger;
        private readonly IMapper _mapper;
        public CreateTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<CreateTeamHandler>();

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

        ///Test.1: Takım oluşturma
        [Fact]
        public async Task Should_CreateTeam_When_PassValidParameters()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team",
                                                members: new List<int>() { 1, 2 },
                                                approvers: new List<int>() { 1 });

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("Test Team", result.TeamName);
            Assert.Equal(2, result.TeamMembers.Count(x => x.IsMember));
            Assert.Equal(1, result.TeamMembers.Count(x => x.IsApprover));
            Assert.Equal(1, result.CreatedBy);
        }

        ///Test.2: Geçersiz userId ile takım oluşturma
        [Fact]
        public async Task Should_ThrowException_When_PassInvalidUserId()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team",
                                                members: new List<int>() { -11, 2 },
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

        ///Test.3: Başka bir firmanın userId'si ile takım oluşturma
        [Fact]
        public async Task Should_ThrowException_When_PassValidUserIdForDifferentCompany()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team",
                                                members: new List<int>() { 1, 3 }, // UserId: 3 belongs to companyId: 1
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

        ///Test.4: Aynı isimli takım oluşturma. Hata fırlatması lazım.
        [Fact]
        public async Task Should_ThrowException_When_PassExistingTeamName()
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

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team",
                                                members: new List<int>() { 1, 2 }, // UserId: 3 belongs to companyId: 1
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

        ///Test.5: Bir üye birden fazla takıma ait olabilir. (%50-%50 çalışma durumu)
        [Fact]
        public async Task Should_CreateTeam_When_PassValidUserIdForDifferentTeam()
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

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team - 2",
                                                members: new List<int>() { 1, 2 },
                                                approvers: new List<int>() { 1 });

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            var user = await userRepository.Queryable().SingleOrDefaultAsync(x => x.UserId == 1);
            // Assert
            Assert.Equal(2, user.TeamMembers.Count);
        }

        ///Test.6: En az bir approver olması lazım
        [Fact]
        public async Task Should_ValidatorReturnFalse_When_EmptyApprover()
        {
            TeamCommandValidator validator = new TeamCommandValidator();
            // Arrange

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<Team> repository = new Repository<Team>(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);

            var handler = new CreateTeamHandler(unitOfWork, repository, userRepository, _logger, _mapper);

            var request = new CreateTeamCommand(companyId: 1,
                                                userId: 1,
                                                "Test Team",
                                                members: new List<int>() { 1, 2 },
                                                approvers: new List<int>() { 1 });

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.True(result.IsValid);
        }
    }
}
