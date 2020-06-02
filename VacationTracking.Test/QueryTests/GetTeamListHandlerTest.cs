using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Queries.Team;
using VacationTracking.Service.Queries.Team;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetTeamListHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetTeamListHandler> _logger;
        private readonly IMapper _mapper;
        public GetTeamListHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetTeamListHandler>();

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
        public async Task Should_ReturnTeamList_When_PassValidCompanyId()
        {
            // Arrange
            IRepository<TeamDb> teamRepository = new Repository<TeamDb>(_fixture.Context);

            var getTeamListHandler = new GetTeamListHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var teamList = await getTeamListHandler.Handle(queryRequest, tcs);

            // Assert
            Assert.NotEmpty(teamList);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassValidCompanyId()
        {
            // Arrange
            IRepository<TeamDb> teamRepository = new Repository<TeamDb>(_fixture.Context);

            var getTeamListHandler = new GetTeamListHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamListQuery(companyId: 3);

            // Act
            var tcs = new CancellationToken();

            var teamList = await getTeamListHandler.Handle(queryRequest, tcs);

            // Assert
            Assert.Empty(teamList);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassInvalidCompanyId()
        {
            // Arrange
            IRepository<TeamDb> teamRepository = new Repository<TeamDb>(_fixture.Context);

            var getTeamListHandler = new GetTeamListHandler(teamRepository, _mapper, _logger);

            var queryRequest = new GetTeamListQuery(companyId: -1);

            // Act
            var tcs = new CancellationToken();

            var teamList = await getTeamListHandler.Handle(queryRequest, tcs);

            // Assert
            Assert.Empty(teamList);
        }
    }
}
