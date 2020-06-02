using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.User;
using VacationTracking.Service.Queries.User;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetUserListHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetUserListHandler> _logger;
        private readonly IMapper _mapper;
        public GetUserListHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetUserListHandler>();

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
            IRepository<User> repository = new Repository<User>(_fixture.Context);

            var handler = new GetUserListHandler(repository, _mapper, _logger);

            var request = new GetUserListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassValidCompanyId()
        {
            // Arrange
            var company = new Company()
            {
                CompanyId = 4,
                CompanyName = "Test Company",
                CreatedAt = DateTime.Now,
                CreatedBy = -1
            };
            _fixture.Context.Add(company);
            _fixture.Context.SaveChanges();

            IRepository<User> repository = new Repository<User>(_fixture.Context);

            var handler = new GetUserListHandler(repository, _mapper, _logger);

            var request = new GetUserListQuery(companyId: 4);

            // Act
            var tcs = new CancellationToken();

            var response = await handler.Handle(request, tcs);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassInvalidCompanyId()
        {
            // Arrange
            IRepository<User> repository = new Repository<User>(_fixture.Context);

            var handler = new GetUserListHandler(repository, _mapper, _logger);

            var request = new GetUserListQuery(companyId: -1);

            // Act
            var tcs = new CancellationToken();

            var response = await handler.Handle(request, tcs);

            // Assert
            Assert.Empty(response);
        }

    }
}
